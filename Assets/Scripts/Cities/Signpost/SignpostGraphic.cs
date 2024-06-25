
public class SignpostGraphic : ACityGraphic
{
    public override void Initialize()
    {
        gameObject.SetActive(false);
        foreach(var item in _graphicSide)
            item.gameObject.SetActive(false);
    }

    public override void AddLink(LinkType type) => _graphicSide[type].gameObject.SetActive(true);

    public override void RoadBuilt(LinkType type, int countFreeLink)
    {
       
    }
}
