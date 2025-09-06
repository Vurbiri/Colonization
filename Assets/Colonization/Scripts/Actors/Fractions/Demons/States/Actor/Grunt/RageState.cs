using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon
    {
        sealed public partial class GruntStates
        {
            sealed private class RageState : ASpecMoveState
            {
                private readonly Chance _chance;

                public new bool CanUse => Moving.IsValue;

                public RageState(SpecSkillSettings specSkill, float speed, ADemonSpecMoveStates parent) : base(specSkill, speed, parent)
                {
                    _chance = new(specSkill.Value);
                }

                protected override bool TryGetTarget(out Hexagon targetHex, out Key direction)
                {
                    targetHex = null; direction = new();
                    if (Moving.IsValue && !NearNoWarriors(CurrentHex) && _chance.Roll)
                    {
                        for (int i = 0; i < HEX.SIDES; i++)
                            if (IsEnter(ref targetHex, direction = HEX.NEAR[_indexes[i]]))
                                break;
                    }

                    return targetHex != null;

                    #region Local IsEnter(..)
                    // ================================================
                    bool IsEnter(ref Hexagon targetHex, Key direction)
                    {
                        Hexagon temp = GameContainer.Hexagons[CurrentHex.Key + direction];
                        while (temp.CanDemonEnter)
                        {
                            if (!NearNoWarriors(temp))
                            {
                                targetHex = temp;
                                _indexes.Shuffle();
                                return true;
                            }

                            temp = GameContainer.Hexagons[temp.Key + direction];
                        }
                        return false;
                    }
                    // ================================================
                    #endregion
                }
            }
        }
    }
}
