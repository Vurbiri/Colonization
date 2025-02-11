//Assets\Colonization\Scripts\Characteristics\Abilities\Interface\IReadOnlyAbilities.cs
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    public interface IReadOnlyAbilities<TId> : IReadOnlyListReactive<TId, int> where TId : AbilityId<TId>
    {
        public bool IsGreater(Id<TId> stateId, int value);
        public bool IsLess(Id<TId> stateId, int value);

        public bool IsTrue(Id<TId> stateId);
    }
}
