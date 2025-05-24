using System.Collections.Generic;

namespace Vurbiri.CreatingMesh
{
    public interface IPrimitive
    {
        public IReadOnlyList<Triangle> Triangles { get; }
    }
}
