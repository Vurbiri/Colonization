//Assets\Colonization\Scripts\Island\IslandCreator\HexCreator.cs
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    using static CONST;

    public abstract class HexCreator
	{
        private readonly Vector2 _offsetHex = new(HEX_DIAMETER_IN, HEX_DIAMETER_IN * SIN_60);

        protected Hexagons _land;
		protected GameplayStorage _storage;
            			
		public HexCreator(Hexagons land, GameplayStorage storage)
		{
			_land = land;
			_storage = storage;
		}

        public static HexCreator Factory(Hexagons land, GameplayStorage storage)
        {
            if(storage.Load) return new HexLoader(land, storage);
                              return new HexGenerator(land, storage);
        }

        public abstract Hexagon Gate { get; }
        public abstract Hexagon Create(Vector3 position, int circle, bool isNotApex);
		public abstract void Finish();

        protected Key PositionToKey(Vector3 position) => new(2f * position.x / _offsetHex.x, position.z / _offsetHex.y);
    }
    //==========================================================================
    sealed public class HexGenerator : HexCreator
    {
        private readonly ShuffleLoopArray<int> _groundIDs, _waterIDs, _surfaceIDs;
        private Chance _chanceWater = CHANCE_WATER;
        private bool _isWater = false;

        public HexGenerator(Hexagons land, GameplayStorage storage) : base(land, storage)
        {
            _groundIDs = new(HEX_IDS); 
            _waterIDs = new(HEX_IDS);
            _surfaceIDs = new((new int[SurfaceId.CountGround]).FillIncrement());
        }

        public override Hexagon Gate
        {
            get
            {
                Hexagon hex = _land.CreateHexagon(Key.Zero, GATE_ID, SurfaceId.Gate, Vector3.zero);
                _storage.HexagonsBind(_land);
                return hex;
            }
        }
        public override Hexagon Create(Vector3 position, int circle, bool isNotApex)
        {
            Key keyHex = PositionToKey(position);
            _isWater = circle == MAX_CIRCLES || (circle == (MAX_CIRCLES - 1) & !_isWater & isNotApex && _chanceWater.Roll);

            if (_isWater) return _land.CreateHexagon(keyHex, _waterIDs.Next,  SurfaceId.Water,  position);
                          return _land.CreateHexagon(keyHex, _groundIDs.Next, _surfaceIDs.Next, position);
        }

        public override void Finish() { }
    }
    //==========================================================================
    sealed public class HexLoader : HexCreator
    {
        public HexLoader(Hexagons land, GameplayStorage storage) : base(land, storage) { }

        public override Hexagon Gate => _land.CreateHexagon(Key.Zero, GATE_ID, SurfaceId.Gate, Vector3.zero);

        public override Hexagon Create(Vector3 position, int circle, bool isNotApex)
        {
            Key keyHex = PositionToKey(position);
            HexLoadData data = _storage.GetHexData(keyHex);

            return _land.CreateHexagon(keyHex, data.id, data.surfaceId, position);
        }

        public override void Finish() => _storage.HexagonsBind(_land);
    }
}
