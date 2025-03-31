//Assets\Colonization\Scripts\Characteristics\Abilities\Interface\IReadOnlyAbilities.cs
using System.Collections.Generic;

namespace Vurbiri.Colonization.Characteristics
{
    public interface IReadOnlyAbilities<TId> : IReadOnlyList<Ability> where TId : AbilityId<TId>
    {
        public Ability this[Id<TId> index] { get; }

        public bool IsGreater(Id<TId> stateId, int value);
        public bool IsLess(Id<TId> stateId, int value);

        public bool IsTrue(Id<TId> stateId);
    }
}
