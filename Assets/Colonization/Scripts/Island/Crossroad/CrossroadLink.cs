using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class CrossroadLink : IValueId<LinkId>
    {
        private readonly Key[] _link = new Key[CrossroadType.Count];
        private readonly Id<LinkId> _id;
        private readonly bool _isNotShore;
        private readonly Vector3 _middle;
        private Id<PlayerId> _owner;

        public Id<LinkId> Id { [Impl(256)] get => _id; }
        public Id<PlayerId> Owner { [Impl(256)] get => _owner; }
        public bool IsOwner { [Impl(256)] get => _owner != PlayerId.None; }
        public bool IsEmpty { [Impl(256)] get => _owner == PlayerId.None; }
        public bool IsNotShore { [Impl(256)] get => _isNotShore; }
        public Key KeyA { [Impl(256)] get => _link[CrossroadType.A]; }
        public Key KeyY { [Impl(256)] get => _link[CrossroadType.Y]; }
        public Vector3 Position { [Impl(256)] get => _middle; }
        
        private CrossroadLink(Id<LinkId> id, Crossroad crossA, Crossroad crossB, bool isNotShore)
        {
            _id = id;
            _link[crossA.Type] = crossA.Key;
            _link[crossB.Type] = crossB.Key;

            _owner = PlayerId.None;
            _isNotShore = isNotShore;
            _middle = (crossA.Position + crossB.Position) * 0.5f;
        }

        public static void Create(List<Crossroad> crossroads, bool isShore)
        {
            Crossroad crossA = crossroads[0], crossB = crossroads[1];
            int id = CROSS.NEAR.IndexOf(crossB.Key - crossA.Key) % LinkId.Count;

            if (!(crossA.Links.ContainsKey(id) && crossB.Links.ContainsKey(id)))
            {
                CrossroadLink link = new(id, crossA, crossB, !isShore);
                crossA.AddLink(link);
                crossB.AddLink(link);
            }
        }

        [Impl(256)] public bool Contains(Key key) => key == KeyA || key == KeyY;
        [Impl(256)] public bool Contains(Crossroad crossroad) => crossroad.Key == _link[crossroad.Type];

        [Impl(256)] public Key Get(Id<CrossroadType> type) => _link[type];
        [Impl(256)] public Key GetOther(Id<CrossroadType> type) => _link[CrossroadType.Max - type];

        [Impl(256)] public Crossroad GetCrossroad(Id<CrossroadType> type) => GameContainer.Crossroads[_link[type]];
        [Impl(256)] public Crossroad GetOtherCrossroad(Id<CrossroadType> type) => GameContainer.Crossroads[_link[CrossroadType.Max - type]];

        [Impl(256)] public void RoadBuilt(Id<PlayerId> playerId)
        {
            _owner = playerId;
            GameContainer.Crossroads[KeyA].RoadBuilt(_id);
            GameContainer.Crossroads[KeyY].RoadBuilt(_id);
        }
        
        [Impl(256)] public void RoadRemove()
        {
            _owner = PlayerId.None;
            GameContainer.Crossroads[KeyA].RoadRemove(_id);
            GameContainer.Crossroads[KeyY].RoadRemove(_id);
        }
    }
}
