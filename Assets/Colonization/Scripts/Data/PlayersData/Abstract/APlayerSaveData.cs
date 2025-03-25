//Assets\Colonization\Scripts\Data\PlayersData\Abstract\APlayerSaveData.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Data
{
    using static SAVE_KEYS;

    public abstract class APlayerSaveData : IDisposable
    {
        protected List<int[][]> _actors = new(DEFOULT_COUNT_KEYS_ACTORS);
        protected readonly List<string> _keysActors = new(DEFOULT_COUNT_KEYS_ACTORS);

        protected readonly IStorageService _storage;
        protected readonly string _strId;
        protected readonly string _keyArtefact;
        protected Unsubscribers _unsubscribers = new();

        private readonly Coroutines _coroutines;
        private Coroutine _saveActors;
        private readonly WaitForSecondsRealtime _delaySave = new(0.75f);

        public APlayerSaveData(int id, IStorageService storage, bool isLoad)
        {
            _storage = storage;
            _coroutines = SceneServices.Get<Coroutines>();

            _strId = id.ToString();
            _keyArtefact = P_BUFFS.Concat(_strId); 

            for (int i = 0; i < DEFOULT_COUNT_KEYS_ACTORS; i++)
            {
                _keysActors.Add(P_ACTORS.Concat(_strId, i.ToString()));
                if (isLoad && storage.TryGet(_keysActors[i], out int[][] actor))
                    _actors.Add(actor);
            }
        }

        public void ActorsBind(IReactiveSet<Actor> actors)
        {
            _unsubscribers += actors.Subscribe(OnActors);

            #region Local OnActors(..)
            //==============================
            void OnActors(Actor actor, TypeEvent operation)
            {
                string key = _keysActors[actor.Index];
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
                    default:
                        return;
                }

            }
            //==============================
            string GetNewKey(int index)
            {
                if(index < _keysActors.Count)
                    return _keysActors[index];

                for (int i = _keysActors.Count - 1; i <= index; i++)
                    _keysActors.Add(P_ACTORS.Concat(_strId, i.ToString()));

                return _keysActors[index];
            }
            #endregion
        }
        public void ArtefactBind(IReactive<IReadOnlyList<int>> currencies, bool calling)
        {
            _unsubscribers += currencies.Subscribe(value => _storage.Set(_keyArtefact, value), calling);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }
    }
}
