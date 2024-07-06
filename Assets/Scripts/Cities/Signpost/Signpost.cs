using UnityEngine;

public class Signpost : City
{
    public override bool Build(City prefab, PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, out City city)
    {
        Debug.Log(owner);
        _prefabUpgrade = prefab;
        _owner = owner;
        if(Upgrade(links, out city))
            return true;

        _owner = PlayerType.None;
        return false;
    }

    public override void Show(bool isShow) => _graphic.gameObject.SetActive(isShow);
}
