using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrentMax : ACurrentMax<ReactiveCombination<int, int>>
    {
        public void Init(IReactive<int> current, IReactive<int> max)
        {
            base.Init();
            _reactiveCurrentMax = new(current, max, SetCurrentMax);
        }
    }
}
