using UnityEngine;

namespace Vurbiri.Colonization
{
    public class CrossroadLink : IValueId<LinkId>
    {
        private static readonly Key[] s_nearCross = { new(2, -1), new(2, 1), new(0, 2), new(-2, 1), new(-2, -1), new(0, -2) };

        private Crossroad _start, _end;
        private Id<PlayerId> _owner;
        private readonly Id<LinkId> _id;
        private readonly bool _isWater;
        private readonly Vector3 _middle;

        public Id<LinkId> Id => _id;
        public bool IsWater => _isWater;
        public Vector3 Position => _middle;
        public Id<PlayerId> Owner { get => _owner; set => _owner = value; }

        public Crossroad Start => _start;
        public Crossroad End => _end;

        private CrossroadLink(Id<LinkId> id, Crossroad start, Crossroad end, bool isWater)
        {
            _id = id;
            _start = start;
            _end = end;

            _owner = PlayerId.None;
            _isWater = isWater;
            _middle = (_start.Position + _end.Position) * 0.5f;
        }

        public static void Create(Crossroad[] crossroads, bool isWater)
        {
            Crossroad start = crossroads[0], end = crossroads[1];
            int id = ToLinkType(end.Key - start.Key);

            if (start.ContainsLink(id) && end.ContainsLink(id))
                return;

            CrossroadLink link = new(id, start, end, isWater);

            start.AddLink(link);
            end.AddLink(link);

            // Local: ToLinkType(..)
            //=================================
            static int ToLinkType(Key key) => System.Array.IndexOf(s_nearCross, key) % 3;
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
            _start.RoadBuilt(_id);
            _end.RoadBuilt(_id);
        }
        public void RoadRemove()
        {
            _owner = PlayerId.None;
            _start.RoadRemove(_id);
            _end.RoadRemove(_id);
        }

        public Crossroad Other(Crossroad crossroad) => crossroad == _start ? _end : _start;

        public bool Contains(Key key) => key == _start.Key || key == _end.Key;

        public override string ToString() => $"({_id}: [{_start.Key} -> {_end.Key}])";
    }
}
