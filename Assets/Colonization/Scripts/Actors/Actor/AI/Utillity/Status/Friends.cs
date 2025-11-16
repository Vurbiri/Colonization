using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI
        {
            protected class Friends : Actors
            {

                public void Update(Actor actor, ReadOnlyArray<Key> keys)
                {
                    Key current = actor._currentHex.Key;
                    for (int i = 0; i < keys.Count; ++i)
                        if (GameContainer.Hexagons.TryGet(current + keys[i], out Hexagon hex) && hex.TryGetFriend(actor._owner, out Actor friend) && friend.IsInCombat())
                            _list.Add(friend, GameContainer.Actors.MaxForce - friend.CurrentForce);
                }

                public void Update(Actor actor)
                {
                    var hexagons = actor._currentHex.Neighbors;
                    for (int i = 0; i < HEX.SIDES; ++i)
                        if (hexagons[i].TryGetEnemy(actor._owner, out Actor friend) && friend.IsInCombat())
                            _list.Add(friend, GameContainer.Actors.MaxForce - friend.CurrentForce);
                }

                [Impl(256)] public void GetNearSafeHexagon(Actor actor, ref int distance, ref Hexagon target) => GetNearHexagon(actor, ref distance, ref target, false);
                [Impl(256)] public void GetNearHexagon(Actor actor, ref int distance, ref Hexagon target) => GetNearHexagon(actor, ref distance, ref target, true);

                private void GetNearHexagon(Actor actor, ref int distance, ref Hexagon target, bool enemyNear)
                {
                    Hexagon current;
                    ReadOnlyArray<Hexagon> hexagons;

                    for (int i = _list.Count - 1; i >= 0; --i)
                    {
                        hexagons = _list[i]._currentHex.Neighbors;
                        for (int h = 0; h < Crossroad.HEX_COUNT; ++h)
                        {
                            current = hexagons[h];
                            if (current.CanWarriorEnter && (enemyNear || !current.IsEnemyNear(actor._owner)) && TryGetDistance(actor, current, distance, out int newDistance))
                            {
                                distance = newDistance;
                                target = current;
                            }
                        }
                    }
                }
            }
        }
    }
}
