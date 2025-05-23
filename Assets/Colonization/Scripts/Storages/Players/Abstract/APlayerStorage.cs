//Assets\Colonization\Scripts\Storages\Players\Abstract\APlayerStorage.cs
using System;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Storage
{
    using static SAVE_KEYS;

    public abstract class APlayerStorage : IDisposable
    {
        protected List<string> _keysActors;

        protected readonly IStorageService _storage;
        protected readonly string _strId;
        protected readonly string _keyArtefact;
        protected Unsubscriptions _unsubscribers = new();

        public APlayerStorage(int id, IStorageService storage)
        {
            _storage = storage;

            _strId = id.ToString();
            _keyArtefact = P_BUFFS.Concat(_strId); 
        }

        public void BindActors(IReactiveSet<Actor> actors)
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
            #endregion
        }
        public void BindArtefact(IReactive<IReadOnlyList<int>> currencies, bool instantGetValue)
        {
            _unsubscribers += currencies.Subscribe(value => _storage.Set(_keyArtefact, value), instantGetValue);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
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

        protected abstract string GetNewKey(int index);
    }
}
