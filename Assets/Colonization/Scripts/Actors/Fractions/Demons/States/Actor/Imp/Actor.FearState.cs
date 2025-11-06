namespace Vurbiri.Colonization
{
    sealed public partial class Demon
    {
        sealed public partial class ImpStates
        {
            sealed private class FearState : ASpecMoveState
            {
                private readonly int _hpOffset;
                private bool _canUse;

                public override bool CanUse => _canUse = Moving.IsTrue && !(Chance.Rolling(HP.Percent + _hpOffset) || NearNoWarriors(CurrentHex));

                public FearState(SpecSkillSettings specSkill, float speed, ADemonSpecMoveStates parent) : base(specSkill, speed, parent)
                {
                    _hpOffset = specSkill.Value;
                }

                protected override bool TryGetTarget(out Hexagon targetHex, out Key direction)
                {
                    targetHex = null; direction = new();
                    if (_canUse)
                    {
                        Key currentKey = CurrentHex.Key;
                        Hexagon temp;
                        foreach (var key in HEX.NEAR_RND)
                        {
                            direction = key;
                            temp = GameContainer.Hexagons[currentKey + direction];
                            if (temp.CanDemonEnter)
                            {
                                temp = GameContainer.Hexagons[temp.Key + direction];
                                if (temp.CanDemonEnter && NearNoWarriors(temp))
                                {
                                    targetHex = temp;
                                    break;
                                }
                            }
                        }
                        _canUse = false;
                    }

                    return targetHex != null;
                }
            }
        }
    }
}
