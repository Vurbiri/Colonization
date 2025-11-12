using System.Collections;

namespace Vurbiri.Colonization
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
                    yield return Skin.SpecSpawn();

                    Actor target;
                    var neighbors = CurrentHex.Neighbors;
                    for (int i = 0; i < neighbors.Count; ++i)
                    {
                        if (neighbors[i].IsWarrior)
                        {
                            target = neighbors[i].Owner;
                            _effects.Apply(Actor, target);
                            target.Skin.Impact(null);
                        }
                    }

                    GetOutOfThisState();
                }
            }  
        }
    }
}
