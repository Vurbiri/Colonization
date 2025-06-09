using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrentMax : ACurrentMax<ReactiveCombination<int, int>>
    {
        public void Init(IReactiveValue<int> current, IReactiveValue<int> max, ProjectColors colors, CanvasHint hint)
        {
            base.Init(colors, hint);
            _reactiveCurrentMax = new(current, max, SetCurrentMax);
        }
    }
}
