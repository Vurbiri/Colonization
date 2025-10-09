using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            sealed private class LandBuild : Plan
            {
                private readonly ReadOnlyMainCurrencies _costEdifice, _costRoad;
                private readonly Step[] _steps;
                private readonly int _count;
                private int _cursor;

                private LandBuild(Builder parent, Crossroad crossroad) : base(parent)
                {
                    _count = 0; _cursor = 0;
                    _steps = new Step[] { new (crossroad) };
                    _costEdifice = GameContainer.Prices.Edifices[crossroad.NextId];
                    _weight = crossroad.Weight + GetEdificeWeight(crossroad.NextId) + parent.GetCostWeight(_costEdifice);
                    if (crossroad.NextGroupId == EdificeGroupId.Colony)
                        _weight += parent.GetProfitWeight(crossroad.Hexagons);
                }

                public override bool IsValid
                {
                    get
                    {
                        bool isValid = FreeRoadCount >= _count && _steps[_cursor].crossroad.IsRoadConnect(Id); 
                        for (int i = _cursor; isValid & i < _count; i++ )
                            isValid = _steps[i].link.IsEmpty;

                        return isValid && _steps[_count].crossroad.CanBuild(Id);
                    }
                }


                public override IEnumerator Appeal_Cn()
                {
                    if (!_done)
                    {
                        Step step;
                        while (_cursor < _count && Human.Exchange(_costRoad))
                        {
                            step = _steps[_cursor];
                            yield return GameContainer.CameraController.ToPositionControlled(step.link.Position);
                            yield return Human.BuyRoad(step.crossroad.Type, step.link, _costRoad);
                            _steps[_cursor++] = null;
                        }
                        yield return null;
                        if(_cursor == _count && Human.Exchange(_costEdifice))
                        {
                            var crossroad = _steps[_cursor].crossroad;
                            yield return GameContainer.CameraController.ToPositionControlled(crossroad.Position);
                            yield return Human.BuyEdificeUpgrade(crossroad, _costEdifice);
                            _done = true;
                        }
                    }
                    yield break;
                }

                public static IEnumerator Create(Builder parent, List<Plan> plans)
                {
                    bool isColony = parent.Abilities.IsGreater(HumanAbilityId.MaxColony, parent.Colonies.Count);
                    bool isShrine = s_shrinesCount < HEX.VERTICES;

                    if (!(isColony | isShrine)) 
                        yield break;

                    HashSet<Crossroad> starts = new();

                    var crossroad = GameContainer.Crossroads.GetRandomPort();
                    var cost = GameContainer.Prices.Edifices[crossroad.NextId];
                    int weight = crossroad.Weight + GetEdificeWeight(crossroad.NextId) + parent.GetCostWeight(cost);
                }

                private class Step
                {
                    public readonly Crossroad crossroad;
                    public readonly CrossroadLink link;

                    public Step(Crossroad crossroad, CrossroadLink link)
                    {
                        this.crossroad = crossroad;
                        this.link = link;
                    }
                    public Step(Crossroad crossroad) => this.crossroad = crossroad;
                }
            }
        }
    }
}
