//Assets\Colonization\Scripts\Island\IslandCreator\HexCreator.cs
using System;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization
{
    using static CONST;

    public abstract class HexCreator : IDisposable
	{
        private readonly Vector2 _offsetHex = new(HEX_DIAMETER_IN, HEX_DIAMETER_IN * SIN_60);

        protected Hexagons _land;
		protected ProjectSaveData _saveData;
            			
		public HexCreator(Hexagons land, ProjectSaveData saveData)
		{
			_land = land;
			_saveData = saveData;
		}

        public static HexCreator Factory(Hexagons land, ProjectSaveData saveData)
        {
            if(saveData.Load) return new HexLoader(land, saveData);
                              return new HexGenerator(land, saveData);
        }

        public abstract Hexagon Gate { get; }
        public abstract Hexagon Create(Vector3 position, int circle, bool isNotApex);
		public abstract void Dispose();

        protected Key PositionToKey(Vector3 position) => new(2f * position.x / _offsetHex.x, position.z / _offsetHex.y);
    }
    //==========================================================================
    public class HexGenerator : HexCreator
    {
        private readonly ShuffleLoopArray<int> _groundIDs, _waterIDs, _surfaceIDs;
        private Chance _chanceWater = CHANCE_WATER;
        private bool _isWater = false;

        public HexGenerator(Hexagons land, ProjectSaveData saveData) : base(land, saveData)
        {
            _groundIDs = new(HEX_IDS); 
            _waterIDs = new(HEX_IDS);
            _surfaceIDs = new((new int[SurfaceId.CountGround]).Fill());
        }

        public override Hexagon Gate
        {
            get
            {
                Hexagon hex = _land.CreateHexagon(Key.Zero, GATE_ID, SurfaceId.Gate, Vector3.zero);
                _saveData.HexagonsBind(_land);
                return hex;
            }
        }
        public override Hexagon Create(Vector3 position, int circle, bool isNotApex)
        {
            Key keyHex = PositionToKey(position);
            Debug.Log(keyHex.Distance);
            _isWater = circle == MAX_CIRCLES || (circle == (MAX_CIRCLES - 1) & !_isWater & isNotApex && _chanceWater.Roll);

            if (_isWater) return _land.CreateHexagon(keyHex, _waterIDs.Next,  SurfaceId.Water,  position);
                            return _land.CreateHexagon(keyHex, _groundIDs.Next, _surfaceIDs.Next, position);
        }

        public override void Dispose() { }
    }
    //==========================================================================
    public class HexLoader : HexCreator
    {
        public HexLoader(Hexagons land, ProjectSaveData saveData) : base(land, saveData) { }

        public override Hexagon Gate => _land.CreateHexagon(Key.Zero, GATE_ID, SurfaceId.Gate, Vector3.zero);

        public override Hexagon Create(Vector3 position, int circle, bool isNotApex)
        {
            Key keyHex = PositionToKey(position);
            _saveData.GetHexData(keyHex, out int id, out int surfaceId);

            return _land.CreateHexagon(keyHex, id, surfaceId, position);
        }

        public override void Dispose() => _saveData.HexagonsBind(_land);
    }
}
