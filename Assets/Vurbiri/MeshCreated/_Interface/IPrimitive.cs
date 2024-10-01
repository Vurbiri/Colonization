using System.Collections.Generic;

namespace Vurbiri.CreatingMesh
{
    public interface IPrimitive
    {
        public IEnumerable<Triangle> Triangles { get; }
    }
}
