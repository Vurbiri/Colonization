using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class CrossroadLink : IValueId<LinkId>
    {
        private readonly Key[] _link = new Key[CrossroadId.Count];
        private readonly Id<LinkId> _id;
        private readonly bool _isWater;
        private readonly Vector3 _middle;
        private Id<PlayerId> _owner;

        public Id<LinkId> Id { [Impl(256)] get => _id; }
        public Id<PlayerId> Owner { [Impl(256)] get => _owner; }
        public bool IsOwner { [Impl(256)] get => _owner != PlayerId.None; }
        public bool IsEmpty { [Impl(256)] get => _owner == PlayerId.None; }
        public bool IsWater { [Impl(256)] get => _isWater; }
        public Vector3 Position { [Impl(256)] get => _middle; }
        
        private CrossroadLink(Id<LinkId> id, Crossroad crossA, Crossroad crossB, bool isWater)
        {
            _id = id;
            _link[crossA.Type] = crossA.Key;
            _link[crossB.Type] = crossB.Key;

            _owner = PlayerId.None;
            _isWater = isWater;
            _middle = (crossA.Position + crossB.Position) * 0.5f;
        }

        public static void Create(List<Crossroad> crossroads, bool isWater)
        {
            Crossroad crossA = crossroads[0], crossB = crossroads[1];
            int id = CROSS.NEAR.IndexOf(crossB.Key - crossA.Key) % 3;

            if (!(crossA.Links.ContainsKey(id) && crossB.Links.ContainsKey(id)))
            {
                CrossroadLink link = new(id, crossA, crossB, isWater);
                crossA.AddLink(link);
                crossB.AddLink(link);
            }
        }

        [Impl(256)] public bool Contains(Key key) => key == _link[CrossroadId.Down] | key == _link[CrossroadId.Up];

        [Impl(256)] public Crossroad Get(Id<CrossroadId> type) => GameContainer.Crossroads[_link[type]];
        [Impl(256)] public Crossroad GetOther(Id<CrossroadId> type) => GameContainer.Crossroads[_link[CrossroadId.Max - type]];

        [Impl(256)] public void RoadBuilt(Id<PlayerId> playerId)
        {
            _owner = playerId;
            for(int i = 0; i < CrossroadId.Count; i++)
                GameContainer.Crossroads[_link[i]].RoadBuilt(_id);
        }
        
        [Impl(256)] public void RoadRemove()
        {
            _owner = PlayerId.None;
            for (int i = 0; i < CrossroadId.Count; i++)
                GameContainer.Crossroads[_link[i]].RoadRemove(_id);
        }
    }
}
