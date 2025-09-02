using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon
    {
        sealed public partial class ImpStates
        {
            sealed private class FearState : ASpecMoveState
            {
                private readonly RandomSequence _indexes = new(HEX.SIDES);
                private readonly int _hpOffset;
                private bool _canUse;

                public new bool CanUse => _canUse = base.CanUse && Moving.IsValue;
                //public new bool CanUse => _canUse = true;

                public FearState(SpecSkillSettings specSkill, float speed, ADemonSpecMoveStates parent) : base(specSkill, speed * 1.25f, parent)
                {
                    _hpOffset = specSkill.Value;
                }

                protected override bool TryGetTarget(out Hexagon targetHex, out Key direction)
                {
                    targetHex = null; direction = new();
                    if (_canUse && !(Chance.Rolling(HP.Percent + _hpOffset) || CurrentHex.NearNoWarriors()))
                    {
                        Key currentKey = CurrentHex.Key;
                        Hexagon temp;
                        for (int i = 0; i < HEX.SIDES; i++)
                        {
                            direction = HEX.NEAR[_indexes[i]];
                            temp = GameContainer.Hexagons[currentKey + direction];
                            if (temp.CanDemonEnter)
                            {
                                temp = GameContainer.Hexagons[temp.Key + direction];
                                if (temp.CanDemonEnter && temp.NearNoWarriors())
                                {
                                    targetHex = temp;
                                    _indexes.Shuffle();
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
