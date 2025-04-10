//Assets\Vurbiri.UI\Runtime\Utility\TargetGraphic.cs
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    public partial class VSelectable
	{
        [Serializable]
        protected class TargetGraphic
        {
            [SerializeField] private Graphic _graphic;
            [SerializeField] private EnumFlags<SelectionState> _stateFilter = true;

            public bool IsValid => _graphic != null && _stateFilter != EnumFlags<SelectionState>.None;
            public bool IsNotNull => _graphic != null;
            public Graphic Graphic => _graphic;
            internal EnumFlags<SelectionState> Filter => _stateFilter;

            public TargetGraphic() { }
            public TargetGraphic(Graphic graphic) 
            {   
                _graphic = graphic;
                _stateFilter = _graphic != null;
            }

            public void SetColor(int state, Color targetColor)
            {
                if (_stateFilter[state])
                    _graphic.canvasRenderer.SetColor(targetColor);
            }
            public void SetColor(SelectionState state, Color targetColor)
            {
                if (_stateFilter[state])
                    _graphic.canvasRenderer.SetColor(targetColor);
            }

            public void CrossFadeColor(int state, Color targetColor, float duration)
			{
                if (_stateFilter[state]) 
                    _graphic.CrossFadeColor(targetColor, duration, true, true);
            }
            public void CrossFadeColor(SelectionState state, Color targetColor, float duration)
            {
                if (_stateFilter[state])
                    _graphic.CrossFadeColor(targetColor, duration, true, true);
            }

            public static implicit operator TargetGraphic(Graphic graphic) => new(graphic);
            public static implicit operator Graphic(TargetGraphic target) => target._graphic;

			public static bool operator ==(TargetGraphic target, Graphic graphic) => target._graphic == graphic;
            public static bool operator !=(TargetGraphic target, Graphic graphic) => target._graphic != graphic;
            public static bool operator ==(Graphic graphic, TargetGraphic target) => target._graphic == graphic;
            public static bool operator !=(Graphic graphic, TargetGraphic target) => target._graphic != graphic;

            public override bool Equals(object obj)
            {
                if (obj is null) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj is TargetGraphic target) return _graphic == target._graphic;
                if (obj is Graphic graphic) return _graphic == graphic;

                return false;
            }
            public override int GetHashCode() => _graphic.GetHashCode();
        }
    }
}
