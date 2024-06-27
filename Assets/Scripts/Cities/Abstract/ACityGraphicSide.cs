using UnityEngine;

public abstract class ACityGraphicSide : MonoBehaviour, IValueTypeEnum<LinkType>
{
    [SerializeField] private LinkType _type;
    
    public LinkType Type => _type;

    public virtual void Initialize()
    {
        gameObject.SetActive(false);
    }

    public virtual void AddLink() => gameObject.SetActive(true);

    public abstract void RoadBuilt();
}
