namespace Vurbiri.Colonization
{
    sealed public partial class Demon
    {
        sealed public partial class GruntStates
        {
            sealed private class RageState : ASpecMoveState
            {
                private readonly Chance _chance;
                private bool _canUse;

                public new bool CanUse => _canUse = Moving.IsTrue && NearWarriors(CurrentHex) && _chance.Roll;

                public RageState(SpecSkillSettings specSkill, float speed, ADemonSpecMoveStates parent) : base(specSkill, speed, parent)
                {
                    _chance = new(specSkill.Value);
                }

                protected override bool TryGetTarget(out Hexagon targetHex, out Key direction)
                {
                    targetHex = null; direction = new();
                    if (_canUse)
                    {
                        _canUse = false;
                        foreach (var key in HEX.NEAR_RND)
                            if (IsEnter(ref targetHex, direction = key))
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
                            if (NearWarriors(temp))
                            {
                                targetHex = temp;
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
