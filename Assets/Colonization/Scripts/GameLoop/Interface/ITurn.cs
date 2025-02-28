//Assets\Colonization\Scripts\GameLoop\Interface\ITurn.cs
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public interface ITurn : IReactiveValue<ITurn>
    {
        public int Turn { get; }
        
        public Id<PlayerId> PreviousId { get; }
        public Id<PlayerId> CurrentId { get; }

        public int[] ToArray();
    }  
}
