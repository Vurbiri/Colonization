using System.Collections.Generic;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI
        {
            protected class Status
            {
                public readonly bool isHuman;

                public int percentHP;
                public bool isMove, isSiege, isGuard;

                public int minColonyGuard;

                public readonly Enemies near = new(), nearTwo = new();

                public readonly List<Hexagon> ownerColoniesHex;
                public readonly List<Hexagon> enemiesColoniesHex;

                public readonly List<Id<PlayerId>> enemies = new(3);

                public Status(Id<PlayerId> playerId)
                {
                    if(isHuman = playerId != PlayerId.Satan)
                    {
                        ownerColoniesHex = new(3);
                        enemiesColoniesHex = new(3);
                    }
                }

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

                    ownerColoniesHex.Clear();
                    enemiesColoniesHex.Clear();

                    enemies.Clear();
                }

                public void FindOwnedColoniesHex(Actor actor)
                {
                    minColonyGuard = int.MaxValue;
                    if(isMove & isGuard)
                    {
                        var playerId = actor._owner;
                        var crossroads = actor._currentHex.Crossroads;
                        for (int i = 0, guardCount; i < HEX.VERTICES; i++)
                        {
                            if (crossroads[i].TryGetOwnerColony(out Id<PlayerId> owner) && playerId == owner)
                            {
                                guardCount = SetHexagons(actor, crossroads[i]);
                                if (guardCount > 0)
                                    minColonyGuard = System.Math.Min(minColonyGuard, guardCount);

                            }
                        }
                    }

                    // =========== Local  ==================
                    int SetHexagons(Actor actor, Crossroad crossroad)
                    {
                        var hexagons = crossroad.Hexagons;
                        Key current = actor._currentHex.Key;
                        Hexagon hexagon; int guardCount = 0;
                        for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                        {
                            hexagon = hexagons[i];
                            if (hexagon.IsOwned)
                            {
                                guardCount += hexagon.IsOwner(actor._owner) ? 1 : 0;
                            }
                            else if (hexagon.Distance(current) == 1 && !ownerColoniesHex.Contains(hexagon))
                            {
                                ownerColoniesHex.Add(hexagon);
                            }
                        }
                        return guardCount;
                    }
                }

                public void FindEnemiesColoniesHex(Actor actor)
                {
                    if (isMove & isSiege)
                    {
                        var crossroads = actor._currentHex.Crossroads;

                        for (int i = 0; i < HEX.VERTICES; i++)
                            if (crossroads[i].TryGetOwnerColony(out Id<PlayerId> owner) && GameContainer.Diplomacy.IsEnemy(actor._owner, owner))
                                SetHexagons(actor._currentHex.Key, crossroads[i].Hexagons);
                    }

                    // =========== Local  ==================
                    [Impl(256)] void SetHexagons(Key current, ReadOnlyArray<Hexagon> hexagons)
                    {
                        Hexagon hexagon;
                        for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                        {
                            hexagon = hexagons[i];
                            if (!hexagon.IsOwned && hexagon.Distance(current) == 1 && !enemiesColoniesHex.Contains(hexagon))
                                enemiesColoniesHex.Add(hexagon);
                        }
                    }
                }

                [Impl(256)] private void SetsGuardAndSiegeStatus(Actor actor)
                {
                    isGuard = isSiege = false;
                    if (near.force == 0)
                    {
                        var crossroads = actor._currentHex.Crossroads;
                        if (isHuman)
                        {
                            var playerId = actor._owner;
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
}
