using System.Collections.Generic;
using UnityEngine;

public abstract class ACityGraphic : MonoBehaviour
{
    [SerializeField] protected UnityDictionary<LinkType, GameObject> _billboards;


    public abstract void Setup(ICollection<LinkType> linkTypes);
}
