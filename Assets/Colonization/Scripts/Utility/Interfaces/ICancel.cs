//Assets\Colonization\Scripts\Utility\Interfaces\ICancel.cs
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public interface ICancel
	{
        public IReactiveValue<bool> CanCancel { get; }
        public void Cancel();
    }
}
