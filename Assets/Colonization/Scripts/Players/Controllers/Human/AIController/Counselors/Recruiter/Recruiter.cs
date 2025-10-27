using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;

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
                if(_current == WarriorId.None & !Human.IsMaxWarriors & Colonies.Count > 0)
                    _current = _recruit.Roll;

                if(_current != WarriorId.None)
                {
                    var cost = GameContainer.Prices.Warriors[_current];
                    yield return Human.Exchange_Cn(cost, Out<bool>.Get(out int key));
                    if(Out<bool>.Result(key) && Warrior.AI.TrySetSpawn(s_spawns, Human))
                    {
                        Hexagon hexagon = s_spawns.Rand(); s_spawns.Clear();

                        yield return GameContainer.CameraController.ToPositionControlled(hexagon.Position);
                        yield return Human.Recruiting_Wait(_current, hexagon, cost);

                        Log.Info($"[Recruiter] {HumanId} recruiting [{_current}]");
                        _current = WarriorId.None;
                    }
                }
            }
        }
    }
}
