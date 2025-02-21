//Assets\Colonization\Scripts\GameLoop\Interface\ITurn.cs
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public interface ITurn : IArrayable, IReadOnlyReactive<ITurn>
    {
        public int Turn { get; }
        
        public int PrevId { get; }
        public int CurrentId { get; }

    }  
}
