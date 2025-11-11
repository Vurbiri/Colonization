using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI
        {
            protected class Status
            {
                public int percentHP;
                public bool isMove, isSiege, isGuard;
                public readonly Enemies near = new(), nearTwo = new();
                public readonly List<Id<PlayerId>> enemies = new(3);

                public void Update(Actor actor)
                {
                    percentHP = actor.PercentHP;
                    isMove = actor.Action.CanUsedMoveSkill();

                    near.Update(actor);
                    nearTwo.Update(actor, HEX.NEAR_TWO);

                    SetsGuardAndSiegeStatus(actor);

                    for (int i = 0; i < PlayerId.Count; i++)
                        if (GameContainer.Diplomacy.IsEnemy(actor._owner, i))
                            enemies.Add(i);
                }

                public void Clear()
                {
                    near.enemies.Clear();
                    nearTwo.enemies.Clear();

                    enemies.Clear();
                }

                [Impl(256)] private void SetsGuardAndSiegeStatus(Actor actor)
                {
                    isGuard = isSiege = false;
                    var crossroads = actor._currentHex.Crossroads;
                    var playerId = actor._owner;

                    if (playerId != PlayerId.Satan)
                    {
                        for (int i = 0; !(isGuard & isSiege) & i < HEX.VERTICES; i++)
                        {
                            if (crossroads[i].TryGetOwnerColony(out Id<PlayerId> owner))
                            {
                                isGuard |= playerId == owner;
                                isSiege = isSiege || GameContainer.Diplomacy.IsEnemy(playerId, owner);

                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; !isSiege & i < HEX.VERTICES; i++)
                            isSiege = crossroads[i].IsColony;
                    }
                }

            }
        }
    }
}
