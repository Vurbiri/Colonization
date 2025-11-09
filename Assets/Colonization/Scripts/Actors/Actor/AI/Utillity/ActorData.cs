namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI
        {
            public readonly struct ActorData : System.IEquatable<ActorData>, System.IEquatable<ActorCode>
            {
                public readonly ActorCode code;
                public readonly int force;

                public ActorData(Actor actor)
                {
                    code = actor.Code;
                    force = actor.CurrentForce;
                }
                public ActorData(ActorCode code)
                {
                    this.code = code;
                    force = 0;
                }

                public readonly bool Equals(ActorData other) => code == other.code;
                public readonly bool Equals(ActorCode actor) => code == actor;
                public readonly override bool Equals(object obj) => obj is not null && ((obj is ActorData other && code == other.code) || (obj is ActorCode actor && code == actor));

                public readonly override int GetHashCode() => code.GetHashCode();
            }
        }
    }
}
