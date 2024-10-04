using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class AEdificeSidesGraphic<T> : AEdificeGraphic where T : AEdificeSide 
    {
        [Space]
        [SerializeField] protected IdHashSet<LinkId, T> _graphicSides;

    }
}
