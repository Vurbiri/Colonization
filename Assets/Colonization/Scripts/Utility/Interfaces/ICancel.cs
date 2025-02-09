//Assets\Colonization\Scripts\Utility\Interfaces\ICancel.cs
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public interface ICancel
	{
        public IReadOnlyReactive<bool> IsCancel { get; }
        public void Cancel();
    }
}
