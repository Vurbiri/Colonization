using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class DemonAI
    {
        sealed private class MoveToEnemy : State
        {
            private readonly WeightsList<Actor> _enemies = new(null);
            private Hexagon _targetHexagon;
            private ActorCode _targetEnemy;

            [Impl(256)] public MoveToEnemy(Actor.AI<DemonsAISettings, DemonId, DemonAIStateId> parent) : base(parent) { }

            public override bool TryEnter()
            {
                _targetHexagon = null;
                if (Status.isMove && !(Status.isSiege || Settings.support))
                {
                    int selfForce = Actor.CurrentForce, selfForceRatio = (int)(selfForce * s_settings.enemyForceRatio);
                    for (int i = 0; i < PlayerId.HumansCount; ++i)
                    {
                        foreach (var enemy in GameContainer.Actors[i])
                        {
                            if(selfForceRatio > enemy.CurrentForce && Goals.Enemies.CanAdd(enemy, selfForce))
                            {
                                int distance = GetDistance(Actor, enemy.Hexagon);
                                if (distance > 0 && distance <= s_settings.maxDistanceToEnemy)
                                    _enemies.Add(enemy, 105 - enemy.PercentHP);
                            }
                        }
                    }

                    while(_enemies.Count > 0)
                    {
                        var enemy = _enemies.Roll;
                        if (Goals.Enemies.Add(_targetEnemy = enemy, new(Actor)))
                        {
                            _targetHexagon = enemy.Hexagon;
                            _enemies.Clear();
                        }
                    }
                }
                return _targetHexagon != null;
            }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return Move_Cn(isContinue, 1, _targetHexagon, !_targetHexagon.IsWarrior);
                if (!isContinue && IsEnemyComing)
                    yield return Settings.defense.Use_Cn(Actor, true, false);
            }

            public override void Dispose() 
            {
                if (_targetHexagon != null)
                {
                    _targetHexagon = null;
                    Goals.Enemies.Remove(_targetEnemy, new(Actor.Code));
                }
            }
        }
    }
}
