using System.Collections.Generic;

namespace Vurbiri
{
    public interface IPrimitive
    {
        public IEnumerable<Triangle> Triangles { get; }
    }
}
