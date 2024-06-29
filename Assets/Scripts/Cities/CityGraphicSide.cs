using UnityEngine;

public class CityGraphicSide : MonoBehaviour, IValueTypeEnum<LinkType>
{
    [SerializeField] private LinkType _type;
    [Space]
    [SerializeField] protected RendererSetupGroup _renderersSetupGroup;

    public LinkType Type => _type;

    public virtual void Initialize() => gameObject.SetActive(false);

    public virtual void AddLink() => gameObject.SetActive(true);

    public virtual void SetMaterial(Material material) => _renderersSetupGroup.SetMaterial(material);
}
