using UnityEngine;

public abstract class ACityGraphicSide : MonoBehaviour, IValueTypeEnum<LinkType>
{
    [SerializeField] private LinkType _type;
    
    public LinkType Type => _type;

    public virtual void Initialize()
    {
        gameObject.SetActive(false);
    }

    public abstract void RoadBuilt();
}
