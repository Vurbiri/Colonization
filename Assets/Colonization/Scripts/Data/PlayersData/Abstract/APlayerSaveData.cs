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
        protected List<int[][]> _actors = new(MAX_KEY_ACTORS);
        protected readonly string[] _keyActors = new string[MAX_KEY_ACTORS];

        protected readonly IStorageService _storage;
        protected readonly string /*_keyActors,*/ _keyArtefact;
        protected Unsubscribers _unsubscribers = new();

        private readonly Coroutines _coroutines;
        private Coroutine _saveActors;
        private readonly WaitForSecondsRealtime _delaySave = new(0.75f);

        public APlayerSaveData(int id, IStorageService storage, bool isLoad)
        {
            _storage = storage;
            _coroutines = SceneServices.Get<Coroutines>();

            string strId = id.ToString();
            _keyArtefact = P_BUFFS.Concat(strId);

            for (int i = 0; i < MAX_KEY_ACTORS; i++)
            {
                _keyActors[i] = P_ACTORS.Concat(strId, i.ToString());
                if (isLoad && storage.TryGet(_keyActors[i], out int[][] actor))
                    _actors.Add(actor);
            }
        }

        public void ActorsBind(IListReactiveItems<Actor> actors)
        {
            _unsubscribers += actors.Subscribe(OnActors);

            #region Local OnActors(..)
            //==============================
            void OnActors(Actor actor, TypeEvent operation)
            {
                string key = _keyActors[actor.Index];
                switch (operation)
                {
                    case TypeEvent.Add:
                        _storage.Save(key, actor);
                        return;
                    case TypeEvent.Remove:
                        _storage.Remove(key);
                        return;
                    case TypeEvent.Change:
                        _storage.Set(key, actor);
                        return;
                    default:
                        return;
                }

                //_saveActors ??= _coroutines.Run(SaveActors_Cn(key, actor));

                //#region Local SaveWarriors_Cn()
                ////==============================
                //IEnumerator SaveActors_Cn(string key, Actor actor)
                //{
                //    yield return _delaySave;
                //    _storage.Set(key, _actors);
                //    _saveActors = null;
                //}
                //#endregion
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
