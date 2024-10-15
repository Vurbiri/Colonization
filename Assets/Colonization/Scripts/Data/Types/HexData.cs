using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [JsonArray]
    public class HexData : IEnumerable<int>
    {
        private readonly Id<SurfaceId> _surfaceId;

        public readonly Key key;
        public readonly int id;
        public SurfaceScriptable surface;
        public readonly Vector3 position;

        public HexData(Key key, int id, Vector3 position, SurfaceScriptable surface)
        {
            this.key = key;
            this.id = id;
            this.position = position;
            this.surface = surface;
            _surfaceId = surface.Id;
        }
        public HexData(int[] array, SurfacesScriptable surfaces)
        {
            key = new(array[0], array[1]);
            id = array[2];
            _surfaceId = array[3];
            position = key.HexKeyToPosition();
            surface = surfaces[_surfaceId];
        }

        public IEnumerator<int> GetEnumerator()
        {
            yield return key.X;
            yield return key.Y;
            yield return id;
            yield return _surfaceId.ToInt;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
