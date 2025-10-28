using System.Collections.Generic;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class SwapId : Cast
            {
                private static readonly List<Hexagon> s_good = new(), s_bad = new();

                private SwapId(Caster parent) : base(parent, MilitarySpellId.Type, MilitarySpellId.SwapId) { }
                public static void Create(Caster parent) => new SwapId(parent);

                public override System.Collections.IEnumerator TryCasting_Cn()
                {
                    if(FindHexagons(HumanId))
                    {
                        yield return CanPayOrExchange_Cn(Out<bool>.Get(out int key));
                        if (Out<bool>.Result(key) && Spell.Prep(_param))
                        {
                            Spell.Cast(_param);

                            yield return s_settings.waitBeforeSelecting.Restart();

                            s_good.Rand().Select(MouseButton.Left);

                            yield return s_settings.waitBeforeSelecting.Restart();

                            s_bad.Rand().Select(MouseButton.Left);

                            yield return SpellBook.WaitEndCasting;
                        }
                    }
                    s_good.Clear(); s_bad.Clear();
                    yield break;
                }

                private static bool FindHexagons(Id<PlayerId> playerId)
                {
                    if (GameContainer.Actors[playerId].Count >= 2)
                    {
                        Hexagon hexagon;
                        foreach (Actor actor in GameContainer.Actors[playerId])
                        {
                            hexagon = actor.Hexagon;

                            if (IsOwnedColony(playerId, hexagon.Crossroads))
                            {
                                if(s_badIds.Contains(hexagon.Id))
                                    s_bad.Add(hexagon);
                            }
                            else
                            {
                                if (s_goodIds.Contains(hexagon.Id))
                                    s_good.Add(hexagon);
                            }
                        }
                    }

                    return s_good.Count > 0 & s_bad.Count > 0;

                    // ===== Local =====
                    static bool IsOwnedColony(Id<PlayerId> playerId, ReadOnlyArray<Crossroad> crossroads)
                    {
                        int index = crossroads.Count;
                        while (index --> 0 && !crossroads[index].IsOwnedColony(playerId));
                        return index >= 0;
                    }
                }
            }
        }
    }
}
