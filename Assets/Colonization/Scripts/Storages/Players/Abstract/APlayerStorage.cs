using System;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Storage
{
    using static SAVE_KEYS;

    public abstract class APlayerStorage : IDisposable
    {
        private List<string> _keysActors;

        protected readonly IStorageService _storage;
        protected readonly string _strId;
        protected readonly string _keyArtefact;
        protected Unsubscription _unsubscribers;

        public APlayerStorage(int id, IStorageService storage, int countActors)
        {
            _storage = storage;

            _strId = id.ToString();
            _keyArtefact = P_BUFFS.Concat(_strId); 

            _keysActors = new(countActors);
            for (int i = 0; i < countActors; i++)
                _keysActors.Add(P_ACTORS.Concat(_strId, i.ToString()));
        }

        public void BindActors(ReadOnlyReactiveSet<Actor> actors)
        {
            _unsubscribers += actors.Subscribe(OnActors);

            #region Local OnActors(..)
            //==============================
            void OnActors(Actor actor, TypeEvent operation)
            {
                switch (operation)
                {
                    case TypeEvent.Add:
                        _storage.Save(GetNewKey(actor.Index), actor);
                        return;
                    case TypeEvent.Remove:
                        _storage.Remove(_keysActors[actor.Index]);
                        return;
                    case TypeEvent.Change:
                        _storage.Set(_keysActors[actor.Index], actor);
                        return;
                }
            }
            //==============================
            string GetNewKey(int index)
            {
                for (int i = _keysActors.Count; i <= index; i++)
                    _keysActors.Add(P_ACTORS.Concat(_strId, i.ToString()));

                return _keysActors[index];
            }
            //==============================
            #endregion
        }
        public void BindArtefact(IReactive<Artefact> currencies, bool instantGetValue)
        {
            _unsubscribers += currencies.Subscribe(artefact => _storage.Set(_keyArtefact, artefact.Levels), instantGetValue);
        }

        public void Dispose()
        {
            _unsubscribers?.Dispose();
        }

        protected List<ActorLoadData> InitActors(int max, bool isLoad)
        {
            _keysActors = new(max);
            List<ActorLoadData> actors = new(max);
            for (int i = 0; i < max; i++)
            {
                _keysActors.Add(P_ACTORS.Concat(_strId, i.ToString()));
                if (isLoad && _storage.TryGet(_keysActors[i], out ActorLoadData actor))
                    actors.Add(actor);
            }
            return actors;
        }
    }
}
