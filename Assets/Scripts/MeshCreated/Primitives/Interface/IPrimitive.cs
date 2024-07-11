using System.Collections.Generic;

namespace MeshCreated
{
    public interface IPrimitive
    {
        public IEnumerable<Triangle> Triangles { get; }
    }
}
