using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public abstract class ASpellToggle : VToggleBase<ASpellToggle>
    {
        [SerializeField] protected ASpellPanel _panel;

        protected readonly int _typeId, _id;
        protected readonly int _points;

        public int Type => _typeId;
        public int Id => _id;

        protected ASpellToggle(int type, int id) : base() 
        {
            _typeId = type; _id = id;
            _points = id * (id + 1);
        }

        public void Init(PerkTree perkTree)
        {
           
            perkTree.GetProgress(_typeId).Subscribe(OnInteractable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        sealed protected override void UpdateVisual() => _panel.Switch(_isOn);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        sealed protected override void UpdateVisualInstant() => _panel.SwitchInstant(_isOn);

        private void OnInteractable(int progress)
        {
            Interactable = progress >= _points;
        }

#if UNITY_EDITOR

        public void SetPosition_Ed(Vector2 position)
        {
            transform.localPosition = position;
            _panel?.SetPosition_Ed(position);
        }

        protected override void OnValidate()
        {
            if (_panel == null)
            {
                foreach(var panel in FindObjectsByType<BloodTradePanel>(FindObjectsSortMode.None))
                {
                    if(panel.Type == _typeId & panel.Id == _id)
                    {
                        _panel = panel;
                        break;
                    }
                }
            }

            base.OnValidate();
        }
#endif
    }
}
