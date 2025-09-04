using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon
    {
        sealed public partial class ImpStates
        {
            sealed private class FearState : ASpecMoveState
            {
                private readonly int _hpOffset;

                public FearState(SpecSkillSettings specSkill, float speed, ADemonSpecMoveStates parent) : base(specSkill, speed, parent)
                {
                    _hpOffset = specSkill.Value;
                }

                protected override bool TryGetTarget(out Hexagon targetHex, out Key direction)
                {
                    targetHex = null; direction = new();
                    if (Moving.IsValue && !(Chance.Rolling(HP.Percent + _hpOffset) || CurrentHex.NearNoWarriors()))
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
                    }

                    return targetHex != null;
                }
            }
        }
    }
}
