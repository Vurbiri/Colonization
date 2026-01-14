using System;
using System.Collections.Generic;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using Vurbiri.Storage;

namespace Vurbiri.Colonization.Storage
{
    using static SAVE_KEYS;

    public abstract class APlayerStorage : IDisposable
    {
        private List<string> _keysActors;

        protected readonly IStorageService _storage;
        protected readonly string _strId;
        protected readonly string _keyArtefact;
        protected Subscription _subscriptions;

        public APlayerStorage(int id, IStorageService storage)
        {
            _storage = storage;

            _strId = id.ToStr();
            _keyArtefact = P_BUFFS.Concat(_strId); 
        }

        public void BindActors(ReadOnlyReactiveSet<Actor> actors)
        {
            _subscriptions += actors.Subscribe(OnActors);

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
                    case TypeEvent.Subscribe:
                    case TypeEvent.Change:
                        _storage.Set(_keysActors[actor.Index], actor);
                        return;
                }
            }
            //==============================
            string GetNewKey(int index)
            {
                for (int i = _keysActors.Count; i <= index; ++i)
                    _keysActors.Add(P_ACTORS.Concat(_strId, i.ToString()));

                return _keysActors[index];
            }
            //==============================
            #endregion
        }
        public void BindArtefact(IReactive<Artefact> currencies, bool instantGetValue)
        {
            _subscriptions += currencies.Subscribe(artefact => _storage.Set(_keyArtefact, artefact.Levels), instantGetValue);
        }

        public void Dispose()
        {
            _subscriptions?.Dispose();
        }

        protected void InitActors(int max)
        {
            List<string> keysActors = new(max);
            for (int i = 0; i < max; ++i)
                keysActors.Add(P_ACTORS.Concat(_strId, i.ToStr()));
            _keysActors = keysActors;
        }
        protected List<ActorLoadData> LoadActors(int max)
        {
            string key;
            List<ActorLoadData> actors = new(max); List<string> keysActors = new(max);
            for (int i = 0; i < max; ++i)
            {
                keysActors.Add(key = P_ACTORS.Concat(_strId, i.ToStr()));
                if (_storage.ContainsKey(key))
                    actors.Add(_storage.Extract<ActorLoadData>(key));
            }
            _keysActors = keysActors;
            return actors;
        }
    }
}
