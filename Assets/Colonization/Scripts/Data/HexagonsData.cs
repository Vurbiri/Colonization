using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class HexagonsData : IEnumerable<HexData>
    {
        private readonly Dictionary<Key, HexData> _hexagons;

        private readonly Coroutines _coroutines;
        private readonly IStorageService _storage;
        private SurfacesScriptable _surfaces;

        public SurfacesScriptable Surfaces => _surfaces;

        public HexagonsData(IReadOnlyDIContainer container, SurfacesScriptable surfaces, bool isLoading)
        {
            _hexagons = new(MAX_HEXAGONS);

            _coroutines = container.Get<Coroutines>();
            _storage = container.Get<IStorageService>();

            _surfaces = surfaces;

            HexData hex;
            if (isLoading && _storage.TryGet(SAVE_KEYS.HEX_DATA, out int[][] arrays))
            {
                foreach(var array in arrays)
                {
                    hex = new(array, surfaces);
                    _hexagons.Add(hex.key, hex);
                }
            }
        }

        public void Add(HexData data) => _hexagons[data.key] = data;

        public void Save(bool saveToFile, Action<bool> callback = null) 
                    => _coroutines.Run(_storage.Save_Coroutine(SAVE_KEYS.HEX_DATA, _hexagons.Values, saveToFile, callback));

        public void UnloadSurfaces()
        {
            _surfaces.Dispose();
            _surfaces = null;
        }

        public IEnumerator<HexData> GetEnumerator() => _hexagons.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
