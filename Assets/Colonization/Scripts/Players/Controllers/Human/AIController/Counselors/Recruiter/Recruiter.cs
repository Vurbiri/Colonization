using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

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
                    int warriorId = i;
                    Abilities[HumanAbilityId.WarriorToAbility(i)].Subscribe((value) => AddRecruit(warriorId, value > 0));
                }

                void AddRecruit(int warriorId, bool add)
                {
                    if (add) _recruit.Add(warriorId, s_weights[warriorId]);
                }
            }

            public override IEnumerator Execution_Cn()
            {
                if(_current == WarriorId.None & !Human.IsMaxWarriors & Colonies.Count > 0)
                    _current = _recruit.Value;

                if(_current != WarriorId.None)
                {
                    var cost = GameContainer.Prices.Warriors[_current];
                    yield return Human.Exchange_Cn(cost, Out<bool>.Get(out int key));
                    if(Out<bool>.Result(key) && TrySetSpawn(Ports))
                    {
                        Hexagon hexagon = s_spawns.Rand(); s_spawns.Clear();

                        yield return GameContainer.CameraController.ToPositionControlled(hexagon.Position);
                        yield return Human.Recruiting_Wait(_current, hexagon, cost);
                        Log.Info($"[Recruiter] Player {HumanId} recruiting [{_current}]");
                        _current = WarriorId.None;
                    }
                }

                // ======== Local =========
                static bool TrySetSpawn(ReadOnlyReactiveList<Crossroad> ports)
                {
                    for (int i = 0; i < ports.Count; i++)
                        SetSpawn(ports[i].Hexagons);

                    return s_spawns.Count > 0;

                    // ----- local ------
                    [Impl(256)] static void SetSpawn(List<Hexagon> hexagons)
                    {
                        Hexagon hexagon;
                        for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                        {
                            hexagon = hexagons[i];
                            if(hexagon.CanWarriorEnter)
                                s_spawns.Add(hexagon);
                        }
                    }
                }
            }
        }
    }
}
