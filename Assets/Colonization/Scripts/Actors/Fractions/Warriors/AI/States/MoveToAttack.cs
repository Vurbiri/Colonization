using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToAttack : State
        {
            private Hexagon _targetHexagon;

            [Impl(256)] public MoveToAttack(Actor.AI<WarriorsAISettings, WarriorId, WarriorAIStateId> parent) : base(parent) { }

            public override bool TryEnter()
            {
                _targetHexagon = null;

                if (Status.isMove && IsEnemyComing)
                {
                    var enemies = Status.nighEnemies; Actor enemy;
                    int selfForce = Actor.CurrentForce, enemyForce;

                    do
                    {
                        enemy = enemies.Extract(); enemyForce = enemy.CurrentForce;
                        if (Chance.Rolling((selfForce * selfForce * s_settings.ratioForAttack) / (enemyForce * enemyForce) - (s_settings.ratioForAttack - 10)))
                            _targetHexagon = enemy.Hexagon;
                    }
                    while (_targetHexagon == null && enemies.NotEmpty);
                }

                return _targetHexagon != null;
            }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return Move_Cn(isContinue, 1, _targetHexagon, !_targetHexagon.IsEnemy(OwnerId));
                if (!isContinue && IsEnemyComing)
                    yield return Settings.defense.Use_Cn(Actor, true, true);
            }

            public override void Dispose() => _targetHexagon = null;
        }
    }
}
