using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI
        {
            protected class Enemies
            {
                public readonly WeightsList<Actor> enemies = new(3);
                public int enemiesForce;
                
                public void Update(Actor actor, ReadOnlyArray<Key> keys)
                {
                    enemiesForce = 0;

                    Key current = actor._currentHex.Key;
                    for (int i = 0; i < keys.Count; i++)
                        if (GameContainer.Hexagons.TryGet(current + keys[i], out Hexagon hex) && hex.TryGetEnemy(actor._owner, out Actor enemy))
                            Add(enemy);
                }

                public void Update(Actor actor)
                {
                    enemiesForce = 0;

                    var hexagons = actor._currentHex.Neighbors;
                    for (int i = 0; i < HEX.SIDES; i++)
                        if (hexagons[i].TryGetEnemy(actor._owner, out Actor enemy))
                            Add(enemy);
                }

                [Impl(256)] private void Add(Actor enemy)
                {
                    int currentForce = enemy.CurrentForce;
                    enemiesForce += currentForce;
                    enemies.Add(enemy, GameContainer.Actors.MaxForce - currentForce);
                }
            }
        }
    }
}
