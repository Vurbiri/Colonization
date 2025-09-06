using System.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon
    {
        sealed public partial class BombStates
        {
            sealed private class ExplosionState : AState
            {
                private readonly HitEffects _effects;

                public ExplosionState(SpecSkillSettings specSkill, BombStates parent) : base(parent)
                {
                    _effects = specSkill.HitEffects[0];
                }

                public override void Enter() => StartCoroutine(ApplySkill_Cn());

                private IEnumerator ApplySkill_Cn()
                {
                    IEnumerator wait = Skin.SpecSpawn();

                    yield return wait;

                    Actor target;
                    foreach (var hex in CurrentHex.Neighbors)
                    {
                        if (hex.IsWarrior)
                        {
                            target = hex.Owner;
                            _effects.Apply(Actor, hex.Owner);
                            target.Skin.Impact(null);
                        }
                    }

                    GetOutOfThisState();
                }

            }  
        }
    }
}
