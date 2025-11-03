using System;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        public class Goals
        {

        }

        private readonly struct Target : IEquatable<Target> 
        {
            public readonly Key key;
            public readonly ActorCode code;

            public Target(Crossroad crossroad, Actor actor)
            {
                key = crossroad;
                code = actor;
            }
            public Target(Hexagon hexagon, Actor actor)
            {
                key = hexagon;
                code = actor;
            }

            public bool Equals(Target other) => key == other.key && code == other.code;

            public override int GetHashCode() => HashCode.Combine(key, code);
        }
    }
}
