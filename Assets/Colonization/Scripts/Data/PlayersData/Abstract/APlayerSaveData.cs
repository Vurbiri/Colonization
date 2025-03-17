//Assets\Colonization\Scripts\Data\PlayersData\Abstract\APlayerSaveData.cs
using System;
using System.Collections;
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
        protected readonly List<int[][]> _actors;

        protected readonly IStorageService _storage;
        protected readonly string _keyActors, _keyArtefact;
        protected Unsubscribers _unsubscribers = new();

        private readonly Coroutines _coroutines;
        private Coroutine _saveActors;
        private readonly WaitForSecondsRealtime _delaySave = new(0.75f);

        public APlayerSaveData(int id, IStorageService storage, bool isLoad)
        {
            _storage = storage;
            _coroutines = SceneServices.Get<Coroutines>();

            string strId = id.ToString();
            _keyActors = P_ACTORS.Concat(strId); _keyArtefact = P_BUFFS.Concat(strId);

            if (!(isLoad && storage.TryGet(_keyActors, out _actors)))
                _actors = new();
        }

        public void ActorsBind(IListReactiveItems<Actor> actors)
        {
            _unsubscribers += actors.Subscribe(OnActors, false);

            #region Local OnWarriors(..)
            //==============================
            void OnActors(Actor actor, TypeEvent operation)
            {
                switch (operation)
                {
                    case TypeEvent.Add:
                        _actors.Add(actor.ToArray());
                        break;
                    case TypeEvent.Remove:
                        _actors.RemoveAt(actor.Index);
                        break;
                    case TypeEvent.Change:
                        _actors[actor.Index] = actor.ToArray(_actors[actor.Index]);
                        break;
                    default:
                        return;
                }

                _saveActors ??= _coroutines.Run(SaveActors_Cn());

                #region Local SaveWarriors_Cn()
                //==============================
                IEnumerator SaveActors_Cn()
                {
                    yield return _delaySave;
                    _storage.Set(_keyActors, _actors);
                    _saveActors = null;
                }
                #endregion
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
