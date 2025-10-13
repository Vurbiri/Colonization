using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class Crossroads
    {
        private readonly Dictionary<Key, Crossroad> _crossroads = new(CROSS.MAX);
        private readonly Shore _shore = new();

        private ReadOnlyArray<int> _hexWeight;
        private Transform _container;
       
        private Vector3[] _vertices = new Vector3[HEX.VERTICES];
        private Quaternion[] _angles = { Quaternion.Euler(0f, 180f, 0f), Quaternion.identity };

        public Crossroad this[Key key] { [Impl(256)] get => _crossroads[key]; }

        public int ShoreCount  { [Impl(256)] get => _shore.Count; }

        public Crossroads(Transform container, ReadOnlyIdSet<EdificeId, AEdifice> prefabs)
        {
            _hexWeight = SettingsFile.Load<HexWeight>();
            _container = container;

            Crossroad.Init(prefabs);

            for (int i = 0; i < HEX.VERTICES; i++)
                _vertices[i] = HEX.RADIUS_OUT * CROSS.DIRECTIONS[i];
        }

        public void CrossroadCreate(Vector3 positionHex, Hexagon hex, bool isLastCircle)
        {
            Crossroad cross; Key key; Vector3 position;
            for (int i = 0, type; i < HEX.VERTICES; i++)
            {
                position = _vertices[i] + positionHex;

                key = position.CrossPositionToKey();

                if (!_crossroads.TryGetValue(key, out cross))
                {
                    if (isLastCircle)
                        continue;

                    type = i % CrossroadType.Count;
                    cross = new(type, key, _container, position, _angles[type]);
                    _crossroads.Add(key, cross);
                }

                if (cross.AddHexagon(hex, out bool ending))
                {
                    hex.Crossroads.Add(cross);
                    if (ending)
                    {
                        cross.Setup(_hexWeight);
                        _shore.Add(cross);
                    }
                }
                else
                {
                    _crossroads.Remove(key);
                }
            }
        }

        public void FinishCreate()
        {
            _container = null;
            _hexWeight = null;
            _vertices = null;
            _angles = null;

            _shore.TrimExcess();
        }

        [Impl(256)] public Crossroad GetRandomPort() => _crossroads[_shore.Value];

        [Impl(256)] public bool IsDeadEnd(Key start, Key end, Id<PlayerId> playerId) => _crossroads[start].IsDeadEnd(playerId) || _crossroads[end].IsDeadEnd(playerId);


        #region Nested: Shore
        //***********************************
        private class Shore : WeightsList<Key>
        {
            public Shore() : base(Key.Zero, HEX.SIDES * (CONST.MAX_CIRCLES + HEX.SIDES)) { }

            public void Add(Crossroad crossroad)
            {
                if (crossroad.CanBuildOnShore && HexagonsValid(crossroad.Hexagons))
                {
                    base.Add(crossroad.Key, crossroad.Weight);
                    crossroad.BannedBuild.Add(Remove);
                }
            }

            private bool HexagonsValid(List<Hexagon> hexagons)
            {
                const int maxX = CONST.MAX_CIRCLES << 1; Key key;
                for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                {
                    key = hexagons[i].Key.Abs();
                    if(key.x == maxX || (key.x == key.y & key.x == CONST.MAX_CIRCLES))
                        return false;
                }
                return true;
            }
        }
        #endregion
    }
}
