//Assets\Colonization\Scripts\Utility\Interfaces\ICancel.cs
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public interface ICancel
	{
        public RBool CanCancel { get; }
        public void Cancel();
    }
}
