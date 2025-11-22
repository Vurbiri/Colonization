namespace Vurbiri.Colonization
{
    sealed public partial class Demon
    {
        sealed public partial class GruntStates
        {
            sealed private class RageState : ASpecMoveState
            {
                private readonly Chance _chance;

                public override bool CanUse => base.CanUse && _chance.Roll && TryGetTarget();

                public RageState(SpecSkillSettings specSkill, float speed, ADemonSpecMoveStates parent) : base(specSkill, speed, parent)
                {
                    _chance = specSkill.Value;
                }

                private bool TryGetTarget()
                {
                    _target = null;
                    foreach (var key in HEX.NEAR_RND)
                    if (IsEnter(ref _target, CurrentHex.Key, _direction = key))
                        break;

                    return _target != null;

                    #region Local IsEnter(..)
                    // ================================================
                    static bool IsEnter(ref Hexagon target, Key start, Key direction)
                    {
                        Hexagon temp = GameContainer.Hexagons[start + direction];
                        while (temp.CanDemonEnter)
                        {
                            if (temp.IsEnemyNear(PlayerId.Satan))
                            {
                                target = temp;
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
