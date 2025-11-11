using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToAttack : AIState
        {
            private Hexagon _targetHexagon;

            [Impl(256)] public MoveToAttack(WarriorAI parent) : base(parent) { }

            public override bool TryEnter()
            {
                _targetHexagon = null;

                if (Raider && Status.isMove && IsEnemyComing)
                {
                    var enemies = Status.nearTwo.enemies;

                    do
                    {
                        Actor enemy = enemies.Extract();
                        if (enemy.CurrentForce < Actor.CurrentForce)
                            _targetHexagon = enemy.Hexagon;
                    }
                    while (_targetHexagon == null && enemies.Count > 0);
                }

                return _targetHexagon != null;
            }

            public override void Dispose() => _targetHexagon = null;

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return Move_Cn(isContinue, 1, _targetHexagon, !_targetHexagon.IsEnemy(OwnerId));
                if (!isContinue && IsEnemyComing)
                    yield return Defense_Cn(true, true);
            }
        }
    }
}
