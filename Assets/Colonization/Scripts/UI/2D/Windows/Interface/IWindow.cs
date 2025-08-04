using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public interface IWindow
	{
        public ISubscription OnOpen { get; }
        public ISubscription OnClose { get; }

        public void Init(bool open);

        public void Close();
        public void Open();
        public void Switch();
    }
}
