using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public interface ICancel
	{
        public RBool CanCancel { get; }
        public void Cancel();
    }
}
