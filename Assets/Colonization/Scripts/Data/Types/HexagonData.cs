using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [JsonArray]
    public class HexagonData : IEnumerable<int>
    {
        public Key Key => _key;
        public int Id => _id;
        public SurfaceType Type => _type;
        public SurfaceScriptable Surface { get => _surface; set => _surface = value; }
        public Vector3 Position { get => _position; set => _position = value; }

        private readonly Key _key;
        private readonly int _id;
        private SurfaceScriptable _surface;
        private Vector3 _position;
        private readonly SurfaceType _type;

        public HexagonData(Key key, int id, Vector3 position, SurfaceScriptable surface)
        {
            _key = key;
            _id = id;
            _position = position;
            _surface = surface;
            _type = surface.Type;
        }
        public HexagonData(int[] arr)
        {
            _key = new(arr[0], arr[1]);
            _id = arr[2];
            _type = (SurfaceType)arr[3];
        }

        public IEnumerator<int> GetEnumerator()
        {
            yield return _key.X;
            yield return _key.Y;
            yield return _id;
            yield return (int)_type;
        }

        public (Key, int, SurfaceScriptable) GetValues() => (_key, _id, _surface);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
