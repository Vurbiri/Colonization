using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class PlayerObjects
    {
        public State<PlayerStateId> ExchangeRate => _states.GetState(PlayerStateId.ExchangeRate);

        public bool CanWallBuild => _states.IsTrue(PlayerStateId.IsWall);
        public bool CanRoadBuild => _states.IsGreater(PlayerStateId.MaxRoads, _roads.Count);

        public IReadOnlyReactiveValue<int> GetStateReactive(Id<PlayerStateId> id) => _states[id];

        public bool IsNotMaxEdifice(int id) => _states.IsGreater(EdificeGroupId.ToIdAbility(id), _edifices.values[id].Count);

        

        public bool CanRecruitingWarrior(Id<WarriorId> id) => _states.IsTrue(id.ToState());
        public bool IsNotMaxWarriors() => _states.IsGreater(PlayerStateId.MaxWarrior, WarriorsCount);
    }
}
