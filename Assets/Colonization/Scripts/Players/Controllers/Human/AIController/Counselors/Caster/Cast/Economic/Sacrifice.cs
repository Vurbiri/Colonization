using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Sacrifice : Cast
            {
                private static readonly List<Actor> s_militias = new(CONST.DEFAULT_MAX_WARRIOR);

                private Sacrifice(Caster parent) : base(parent, EconomicSpellId.Type, EconomicSpellId.Sacrifice, parent.IsMilitarist) { }
                public static void Create(Caster parent) => new Sacrifice(parent);

                public override IEnumerator TryCasting_Cn()
                {
                    if (FindMilitias(HumanId) && Spell.Prep(_param))
                    {
                        Actor militia = s_militias.Rand();
                        if (militia.Action.CanUseSkill(s_settings.skillId))
                        {
                            yield return militia.Action.UseSkill(s_settings.skillId);
                            yield return s_settings.waitBeforeSelecting.Restart();
                        }

                        Spell.Cast(_param);

                        yield return s_settings.waitBeforeSelecting.Restart();

                        militia.Select();

                        yield return SpellBook.WaitEndCasting;
                    }
                    s_militias.Clear();
                    yield break;
                }

                [Impl(256)] private static bool FindMilitias(int playerId)
                {
                    foreach (Actor actor in GameContainer.Actors[playerId])
                        if (actor.Id == WarriorId.Militia & actor.IsFullHP & !actor.ZealCharge && !actor.IsInCombat())
                            s_militias.Add(actor);

                    return s_militias.Count > 0;
                }
            }
        }
    }
}
