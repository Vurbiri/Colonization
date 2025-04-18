//Assets\Colonization\Scripts\Players\Player\HumanBuildMethods.cs
using System.Collections;
using Vurbiri.Colonization.Actors;
using static Vurbiri.Colonization.Characteristics.HumanAbilityId;

namespace Vurbiri.Colonization
{
    public partial class Human
    {
        #region Edifice
        public bool CanEdificeUpgrade(Crossroad crossroad) => _edifices.CanEdificeUpgrade(crossroad) && crossroad.CanUpgrade(_id);
        public void BuyEdificeUpgrade(Crossroad crossroad)
        {
            if (crossroad.BuyUpgrade(_id))
            {
                _resources.Pay(_prices.Edifices[crossroad.Id.Value]);
                _edifices.edifices[crossroad.GroupId].AddOrChange(crossroad);
            }
        }

        public bool CanWallBuild(Crossroad crossroad) => _abilities.IsTrue(IsWall) && crossroad.CanWallBuild(_id);
        public void BuyWall(Crossroad crossroad)
        {
            if (crossroad.BuyWall(_id, _abilities[WallDefence]))
            {
                _resources.Pay(_prices.Wall);
                _edifices.edifices[crossroad.GroupId].Signal(crossroad);
            }
        }
        #endregion

        #region Roads
        public bool CanRoadBuild(Crossroad crossroad) => _abilities.IsGreater(MaxRoads, _roads.Count) && crossroad.CanRoadBuild(_id);
        public void BuyRoad(Crossroad crossroad, Id<LinkId> linkId)
        {
            _resources.Pay(_prices.Road);
            _roads.BuildAndUnion(crossroad.GetLinkAndSetStart(linkId));
        }
        #endregion

        #region Warriors
        public bool CanAnyRecruiting(Crossroad crossroad)
        { 
            return _abilities.IsGreater(MaxWarrior, _warriors.Count) && crossroad.CanRecruiting(_id); 
        }
        public bool CanRecruiting(Id<WarriorId> id) => _abilities.IsTrue(id.ToState());

        public void Recruiting(Id<WarriorId> id, Crossroad crossroad) => _coroutines.Run(Recruiting_Cn(id, crossroad));
        public void Recruiting(Id<WarriorId> id, Hexagon hexagon)
        {
            _resources.Pay(_prices.Warriors[id.Value]);
            _warriors.Add(_spawner.Create(id, hexagon));
        }

        private IEnumerator Recruiting_Cn(Id<WarriorId> id, Crossroad crossroad)
        {
            WaitResult<Hexagon> result = crossroad.GetHexagonForRecruiting_Wait();
            yield return result;

            if (result.Value == null)
                yield break;

            Recruiting(id, result.Value);
        }
        #endregion
    }
}
