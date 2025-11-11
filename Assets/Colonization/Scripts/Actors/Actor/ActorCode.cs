using System;

namespace Vurbiri.Colonization
{
	public readonly struct ActorCode : IEquatable<ActorCode>, IEquatable<Actor>
    {
		public readonly int owner;
        public readonly int index;

		public ActorCode(int owner, int index)
		{
			this.owner = owner;
			this.index = index;
		}

        public ActorCode(Actor actor)
        {
            this.owner = actor.Owner.Value;
            this.index = actor.Index;
        }

        public override int GetHashCode() => HashCode.Combine(owner, index);

        public bool Equals(ActorCode other) => other.owner == owner & other.index == index;
        public bool Equals(Actor actor) => actor != null && actor.Owner == owner & actor.Index == index;
        public override bool Equals(object obj) => (obj is ActorCode code && Equals(code)) || (obj is Actor actor && Equals(actor));

        public static bool operator ==(ActorCode lhs, ActorCode rhs) => lhs.owner == rhs.owner & lhs.index == rhs.index;
        public static bool operator !=(ActorCode lhs, ActorCode rhs) => lhs.owner != rhs.owner | lhs.index != rhs.index;

        public static bool operator ==(ActorCode code, Actor actor) => code.Equals(actor);
        public static bool operator !=(ActorCode code, Actor actor) => !code.Equals(actor);

        public static bool operator ==(Actor actor, ActorCode code) => code.Equals(actor);
        public static bool operator !=(Actor actor, ActorCode code) => !code.Equals(actor);
    }
}
