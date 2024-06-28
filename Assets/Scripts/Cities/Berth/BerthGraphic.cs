using UnityEngine;

public class BerthGraphic : CityGraphic
{
    
    private static readonly Quaternion angle0 = Quaternion.identity, angle180 = Quaternion.Euler(0f, 180f, 0f);

    public override void Upgrade(EnumHashSet<LinkType, CrossroadLink> links)
    {
        Initialize();

        Material material = _players.Current.Material;

        if(links.Count == 3)
        {
            foreach (var link in links)
            {
                if (link.IsWater)
                    DestroySide(link.Type);
                else
                    ActiveSide(_graphicSides[link.Type]);
            }

            transform.localRotation = angle180;
        }
        else
        {
            foreach (var link in links)
                DestroySide(link.Type);

            ActiveSide(_graphicSides.First());

            transform.localRotation = angle0;
        }

        #region Local: DestroySide(...), ActiveSide(...)
        //=================================
        void DestroySide(LinkType type)
        {
            Destroy(_graphicSides[type].gameObject);
            _graphicSides.Remove(type);
        }
        //=================================
        void ActiveSide(CityGraphicSide side)
        {
            side.AddLink();
            side.RoadBuilt(material);
        }
        #endregion
    }

    public override void AddLink(LinkType type) {}

    public override void RoadBuilt(LinkType type, PlayerType owner) { }
}
