//Assets\Colonization\Scripts\Characteristics\Abilities\Interface\IAbility.cs
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public interface IAbility : IReactiveValue<int>
    {
        public bool IsValue { get; }
    }
}
