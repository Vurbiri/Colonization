using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class CrossroadLink : IValueId<LinkId>
    {
        private Key _start, _end;
        private readonly Id<LinkId> _id;
        private Id<PlayerId> _owner;
        private readonly bool _isWater;
        private readonly Vector3 _middle;

        public Crossroad Start { [Impl(256)] get => GameContainer.Crossroads[_start]; }
        public Crossroad End { [Impl(256)] get => GameContainer.Crossroads[_end]; }
        public Id<LinkId> Id { [Impl(256)] get => _id; }
        public Id<PlayerId> Owner { [Impl(256)] get => _owner; }
        public bool IsOwner { [Impl(256)] get => _owner != PlayerId.None; }
        public bool IsEmpty { [Impl(256)] get => _owner == PlayerId.None; }
        public bool IsWater { [Impl(256)] get => _isWater; }
        public Vector3 Position { [Impl(256)] get => _middle; }
        
        private CrossroadLink(Id<LinkId> id, Crossroad start, Crossroad end, bool isWater)
        {
            _id = id;
            _start = start.Key;
            _end = end.Key;

            _owner = PlayerId.None;
            _isWater = isWater;
            _middle = (start.Position + end.Position) * 0.5f;
        }

        public static void Create(List<Crossroad> crossroads, bool isWater)
        {
            Crossroad start = crossroads[0], end = crossroads[1];
            int id = CROSS.NEAR.IndexOf(end.Key - start.Key) % 3;

            if (start.ContainsLink(id) && end.ContainsLink(id))
                return;

            CrossroadLink link = new(id, start, end, isWater);

            start.AddLink(link);
            end.AddLink(link);
        }

        [Impl(256)] public CrossroadLink SetStart(Key key)
        {
            if (_start != key) (_start, _end) = (_end, _start);
            return this;
        }

        [Impl(256)] public void RoadBuilt(Id<PlayerId> playerId)
        {
            _owner = playerId;
            GameContainer.Crossroads[_start].RoadBuilt(_id);
            GameContainer.Crossroads[_end].RoadBuilt(_id);
        }
        
        [Impl(256)] public void RoadRemove()
        {
            _owner = PlayerId.None;
            GameContainer.Crossroads[_start].RoadRemove(_id);
            GameContainer.Crossroads[_end].RoadRemove(_id);
        }

        [Impl(256)] public Crossroad Other(Key key) => GameContainer.Crossroads[key == _start ? _end : _start];

        [Impl(256)] public bool Contains(Key key) => key == _start | key == _end;

        [Impl(256)] public override string ToString() => $"({_id}: [{_start} -> {_end}])";
    }
}
