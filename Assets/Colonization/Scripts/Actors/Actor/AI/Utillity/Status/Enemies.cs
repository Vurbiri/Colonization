using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI
        {
            protected class Enemies : Actors
            {
                private int _force;

                public int Force { [Impl(256)] get => _force; }
                public bool IsForce { [Impl(256)] get => _force > 0; }

                public void Update(Actor actor, ReadOnlyArray<Key> keys)
                {
                    _force = 0;

                    Key current = actor._currentHex.Key;
                    for (int i = 0; i < keys.Count; ++i)
                        if (GameContainer.Hexagons.TryGet(current + keys[i], out Hexagon hex) && hex.TryGetEnemy(actor._owner, out Actor enemy))
                            Add(enemy);
                }

                public void Update(Actor actor)
                {
                    _force = 0;

                    var hexagons = actor._currentHex.Neighbors;
                    for (int i = 0; i < HEX.SIDES; ++i)
                        if (hexagons[i].TryGetEnemy(actor._owner, out Actor enemy))
                            Add(enemy);
                }

                public int GetContraForce()
                {
                    int contraForce = 0;
                    for (int i = 0; i < _list.Count; ++i)
                        contraForce += _list[i].GetCurrentForceNearEnemies();
                    return contraForce;
                }

                [Impl(256)] private void Add(Actor enemy)
                {
                    _force += enemy.CurrentForce;
                    _list.Add(enemy);
                }
            }
        }
    }
}
