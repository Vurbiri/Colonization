using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public abstract class AToggleGroup<TToggle> : VToggleGroup<TToggle> where TToggle : VToggleBase<TToggle>
    {
        protected void OnDestroy()
        {
            _onValueChanged.Clear();
            _toggles.Clear();
            _activeToggle = null;
        }
    }
}
