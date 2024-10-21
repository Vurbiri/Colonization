using UnityEngine;

namespace Vurbiri.Colonization
{
    public class CrossroadLink : IValueId<LinkId>
    {
        public Id<LinkId> Id => _id;
        public bool IsWater => _isWater;
        public Vector3 Position => _middle;
        public Id<PlayerId> Owner { get => _owner; set => _owner = value; }

        public Crossroad Start => _start;
        public Crossroad End => _end;

        private Crossroad _start, _end;
        private Id<PlayerId> _owner;
        private readonly Id<LinkId> _id;
        private readonly bool _isWater;
        private readonly Vector3 _middle;

        private static readonly Key[] NEAR_CROSS = { new(2, -1), new(2, 1), new(0, 2), new(-2, 1), new(-2, -1), new(0, -2) };

        public CrossroadLink(Crossroad[] arr, bool isWater)
        {
            _start = arr[0]; _end = arr[1];
            _id = ToLinkType(_end - _start);

            if (!(_start.AddLink(this) && _end.AddLink(this)))
                return;

            _isWater = isWater;
            _owner = PlayerId.None;
            _middle = (_start.Position + _end.Position) * 0.5f;

            // Local: ToLinkType(..)
            //=================================
            static Id<LinkId> ToLinkType(Key key) => new(System.Array.IndexOf(NEAR_CROSS, key) % 3);
        }

        public CrossroadLink SetStart(Crossroad cross)
        {
            if (_start != cross)
                (_start, _end) = (_end, _start);

            return this;
        }

        public void RoadBuilt(Id<PlayerId> playerId)
        {
            _owner = playerId;
            _start.RoadBuilt(_id, playerId);
            _end.RoadBuilt(_id, playerId);
        }

        public Crossroad Other(Crossroad crossroad) => crossroad == _start ? _end : _start;

        public bool Contains(Key key) => key == _start.Key || key == _end.Key;

        public override string ToString() => $"({_id}: [{_start.Key} -> {_end.Key}])";
    }
}
