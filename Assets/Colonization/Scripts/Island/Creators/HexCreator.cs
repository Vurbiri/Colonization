//Assets\Colonization\Scripts\Island\IslandCreator\HexCreator.cs
using UnityEngine;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    using static CONST;

    public abstract class HexCreator
	{
        private readonly Vector2 _offsetHex = new(HEX_DIAMETER_IN, HEX_DIAMETER_IN * SIN_60);

        protected Hexagons _land;
        protected HexagonSpawner _spawner;
        protected GameplayStorage _storage;
            			
		public HexCreator(Hexagons land, HexagonSpawner spawner, GameplayStorage storage)
		{
			_land = land;
            _spawner = spawner;
			_storage = storage;
		}

        public static HexCreator Factory(Hexagons land, HexagonSpawner spawner, GameplayStorage storage)
        {
            if(storage.Load) return new HexLoader(land, spawner, storage);
                             return new HexGenerator(land, spawner, storage);
        }

        public abstract Hexagon Gate { get; }
        public abstract Hexagon Create(Vector3 position, int circle, bool isNotApex);
		public abstract void Finish();

        protected Key PositionToKey(Vector3 position) => new(2f * position.x / _offsetHex.x, position.z / _offsetHex.y);

        protected Hexagon Create(Key key, int id, int surfaceId, Vector3 position)
        {
            return _land.Add(key, id, _spawner.Spawn(key, id, surfaceId, position));
        }
    }
    //==========================================================================
    sealed public class HexGenerator : HexCreator
    {
        private readonly SequenceRandomIds _groundIDs, _waterIDs, _surfaceIDs;
        private Chance _chanceWater = CHANCE_WATER;
        private bool _isWater = false;

        public HexGenerator(Hexagons land, HexagonSpawner spawner, GameplayStorage storage) : base(land, spawner, storage)
        {
            _groundIDs  = new(HEX_IDS); 
            _waterIDs   = new(HEX_IDS);
            _surfaceIDs = new(SurfaceId.CountGround);
        }

        public override Hexagon Gate
        {
            get
            {
                Hexagon hex = Create(Key.Zero, GATE_ID, SurfaceId.Gate, Vector3.zero);
                _storage.HexagonsBind(_land);
                return hex;
            }
        }
        public override Hexagon Create(Vector3 position, int circle, bool isNotApex)
        {
            Key keyHex = PositionToKey(position);
            _isWater = circle == MAX_CIRCLES || (circle == (MAX_CIRCLES - 1) & !_isWater & isNotApex && _chanceWater.Roll);

            if (_isWater) return Create(keyHex, _waterIDs.Next,  SurfaceId.Water,  position);
                          return Create(keyHex, _groundIDs.Next, _surfaceIDs.Next, position);
        }

        public override void Finish() { }
    }
    //==========================================================================
    sealed public class HexLoader : HexCreator
    {
        public HexLoader(Hexagons land, HexagonSpawner spawner, GameplayStorage storage) : base(land, spawner, storage) { }

        public override Hexagon Gate => Create(Key.Zero, GATE_ID, SurfaceId.Gate, Vector3.zero);

        public override Hexagon Create(Vector3 position, int circle, bool isNotApex)
        {
            Key keyHex = PositionToKey(position);
            HexLoadData data = _storage.GetHexData(keyHex);

            return Create(keyHex, data.id, data.surfaceId, position);
        }

        public override void Finish() => _storage.HexagonsBind(_land);
    }
}
