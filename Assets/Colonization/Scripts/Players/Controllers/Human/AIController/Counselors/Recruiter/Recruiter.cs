using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private class Recruiter : Counselor
        {
            private static readonly ReadOnlyIdArray<WarriorId, int> s_weights;
            private static readonly List<Hexagon> s_spawns = new(6);

            static Recruiter() => s_weights = SettingsFile.Load<RecruiterSettings>().weights;

            private readonly WeightsList<Id<WarriorId>> _recruit = new(WarriorId.None, WarriorId.Count);
            private Id<WarriorId> _current = WarriorId.None;

            public Recruiter(AIController parent) : base(parent)
            {
                for (int i = 0; i < WarriorId.Count; i++)
                {
					Id<WarriorId> warriorId = i;
                    Abilities[HumanAbilityId.WarriorToAbility(i)].Subscribe((value) => AddRecruit(warriorId, value > 0));
                }

                void AddRecruit(Id<WarriorId> warriorId, bool add)
                {
                    if (add) _recruit.Add(warriorId, s_weights[warriorId]);
                }
            }

            public override IEnumerator Execution_Cn()
            {
                int warriorsCount = GameContainer.Actors[HumanId].Count;
                if (_current == WarriorId.None)
                {
                    if(Abilities.IsGreater(HumanAbilityId.MaxWarrior, warriorsCount) && (Colonies.Count << 1) > warriorsCount)
                        _current = _recruit.Roll;
                }

                if(_current != WarriorId.None)
                {
                    var cost = GameContainer.Prices.Warriors[_current];
                    yield return Human.Exchange_Cn(cost, Out<bool>.Get(out int key));
                    if(Out<bool>.Result(key))
                    {
                        if (WarriorAI.TrySetSpawn(Human, s_spawns))
                        {
                            Hexagon hexagon = s_spawns.Rand(); s_spawns.Clear();

                            yield return GameContainer.CameraController.ToPositionControlled(hexagon.Position);
                            yield return Human.Recruiting_Wait(_current, hexagon, cost);
#if TEST_AI
                            Log.Info($"[Recruiter] {HumanId} recruiting [{_current}]");
#endif
                            _current = WarriorId.None;
                        }
                    }
                    else if(IsSiege(HumanId, Colonies) || IsSiege(HumanId, Ports))
                    {
                        if (_current != WarriorId.Militia && (Resources.Amount << 1) <= cost.Amount)
                            _current = WarriorId.Militia;
                        if (warriorsCount <= 1)
                            Resources.AddToMin(s_settings.addRes);
                    }

                    // ====== Local ======
                    static bool IsSiege(Id<PlayerId> playerId, ReadOnlyReactiveList<Crossroad> colonies)
                    {
                        for (int i = 0; i < colonies.Count; i++)
                            if (colonies[i].IsEnemyNear(playerId))
                                return true;
                        return false;
                    }
                }
            }
        }
    }
}
