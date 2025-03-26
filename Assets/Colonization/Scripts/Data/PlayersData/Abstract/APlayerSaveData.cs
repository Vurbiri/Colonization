//Assets\Colonization\Scripts\Data\PlayersData\Abstract\APlayerSaveData.cs
using System;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Data
{
    using static SAVE_KEYS;

    public abstract class APlayerSaveData : IDisposable
    {
        protected List<string> _keysActors;

        protected readonly IStorageService _storage;
        protected readonly string _strId;
        protected readonly string _keyArtefact;
        protected Unsubscribers _unsubscribers = new();

        public APlayerSaveData(int id, IStorageService storage)
        {
            _storage = storage;

            _strId = id.ToString();
            _keyArtefact = P_BUFFS.Concat(_strId); 
        }

        public void ActorsBind(IReactiveSet<Actor> actors)
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
                    default:
                        return;
                }

            }
            //==============================
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

        //protected List<int[][]> InitActors(int max, bool isLoad)
        //{
        //    _keysActors = new(max);
        //    List<int[][]> actors = new(max);
        //    for (int i = 0; i < max; i++)
        //    {
        //        _keysActors.Add(P_ACTORS.Concat(_strId, i.ToString()));
        //        if (isLoad && _storage.TryGet(_keysActors[i], out int[][] actor))
        //            actors.Add(actor);
        //    }
        //    if (isLoad)
        //        _storage.Get<ActorLoadData>(_keysActors[0]);
        //    return actors;
        //}

        protected abstract string GetNewKey(int index);
    }
}
