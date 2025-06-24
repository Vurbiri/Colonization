using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrentMax : ACurrentMax<ReactiveCombination<int, int>>
    {
        public void Init(IReactive<int> current, IReactive<int> max, CanvasHint hint)
        {
            base.Init(hint);
            _reactiveCurrentMax = new(current, max, SetCurrentMax);
        }
    }
}
