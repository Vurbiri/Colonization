using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public interface IWindow
	{
        public ISubscription OnOpen { get; }
        public ISubscription OnClose { get; }

        public void Init(Human player, CanvasHint hint, bool open);

        public void Close();
        public void Open();
        public void Switch();
    }
}
