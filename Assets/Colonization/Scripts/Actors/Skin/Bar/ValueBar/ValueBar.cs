using TMPro;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.UI
{
    public class ValueBar : System.IDisposable
    {
        private readonly TextMeshPro _valueTMP;
        private readonly PopupWidget3D _popup;
        private readonly Sprite _sprite;
        private readonly Subscription _subscription;
        private readonly int _shift;

        private int _currentValue = int.MaxValue;

        public ValueBar(int id, TextMeshPro value, PopupWidget3D popup, Sprite sprite, ReadOnlyAbilities<ActorAbilityId> abilities)
        {
            _valueTMP = value; _sprite = sprite; _popup = popup;

            _shift = id <= ActorAbilityId.MAX_ID_SHIFT_ABILITY ? ActorAbilityId.SHIFT_ABILITY : 0;
            _subscription = abilities[id].Subscribe(SetValue);
        }

        public void Dispose() => _subscription.Dispose();

        private void SetValue(int value)
        {
            value >>= _shift;
            _valueTMP.text = value.ToString();

            _popup.Run(value - _currentValue, _sprite);
            _currentValue = value;
        }
	}
}
