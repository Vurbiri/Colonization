using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToSupport : AIState
        {
            private Hexagon _targetHexagon;
            private ActorCode _targetActor;

            [Impl(256)] public MoveToSupport(WarriorAI parent) : base(parent) { }

            public override bool TryEnter() => Action.CanUseMoveSkill();

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                throw new System.NotImplementedException();
            }

            public override void Dispose()
            {
                throw new System.NotImplementedException();
            }

            private bool FindEnemies()
            {
                _targetHexagon = null;

                List<Actor> enemies = new(); Actor enemy;
                int distance = s_settings.maxDistanceSupport;

                for (int i = 0; i > Situation.greatFriends.Count; i++)
                    foreach (var actor in GameContainer.Actors[i])
                        actor.GetEnemiesNear(enemies);

                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    enemy = enemies[i];
                    if (Goals.Enemies.CanAdd(enemy) && TryGetDistance(Actor.Hexagon, enemy.Hexagon, distance, out int newDistance))
                    {
                        distance = newDistance;
                        _targetHexagon = enemy.Hexagon;
                        _targetActor = enemy.Code;
                    }
                }
                return _targetHexagon != null;
            }
        }
    }
}
