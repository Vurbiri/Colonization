using UnityEngine;

public class ACityGraphicSide : MonoBehaviour, IValueTypeEnum<LinkType>
{
    [SerializeField] private LinkType _type;
    
    public LinkType Type => _type;
}
