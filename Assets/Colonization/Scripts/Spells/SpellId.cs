using System;

namespace Vurbiri.Colonization
{
	[Serializable]
	public struct SpellId : IEquatable<SpellId>
	{
		public int type;
		public int id;

		public SpellId(int type, int id)
		{
			this.type = type;
			this.id = id;
		}

        public override readonly int GetHashCode() => HashCode.Combine(type, id);
        public readonly bool Equals(SpellId spellId) => type == spellId.type & id == spellId.id;
        public readonly override bool Equals(object other)
		{
			if (other is SpellId spellId) 
				return type == spellId.type & id == spellId.id;
            return false;
        }

        public static bool operator ==(SpellId a, SpellId b ) => a.type == b.type & a.id == b.id;
        public static bool operator !=(SpellId a, SpellId b) => a.type != b.type | a.id != b.id;
    }
}
