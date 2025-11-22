namespace Vurbiri.Colonization
{
    sealed public partial class Demon
    {
        sealed public partial class ImpStates
        {
            sealed private class FearState : ASpecMoveState
            {
                private readonly int _hpOffset;

                public override bool CanUse => base.CanUse && Chance.Rolling(HP.Percent + _hpOffset) && TryGetTarget();

                public FearState(SpecSkillSettings specSkill, float speed, ADemonSpecMoveStates parent) : base(specSkill, speed, parent)
                {
                    _hpOffset = specSkill.Value;
                }

                private bool TryGetTarget()
                {
                    Key current = CurrentHex.Key;
                    Hexagon temp; _target = null;
                    foreach (var direction in HEX.NEAR_RND)
                    {
                        temp = GameContainer.Hexagons[current + direction];

                        if (temp.CanDemonEnter)
                        {
                            temp = GameContainer.Hexagons[temp.Key + direction];
                            if (temp.CanWarriorEnter && !temp.IsEnemyNear(PlayerId.Satan))
                            {
                                _target = temp;
                                _direction = direction;
                                break;
                            }
                        }
                    }

                    return _target != null;
                }
            }
        }
    }
}
