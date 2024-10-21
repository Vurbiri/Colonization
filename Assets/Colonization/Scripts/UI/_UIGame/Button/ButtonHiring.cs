using UnityEngine;
using Vurbiri.Localization;

namespace Vurbiri.Colonization.UI
{
    public class ButtonHiring : AButtonBuildType<WarriorId>
    {
        public void Init(Vector3 localPosition, ACurrencies cost)
        {
            base.Init(localPosition);
            _cost = cost;
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);
        }

        public void Setup(Color color, ACurrencies cash)
        {
            _button.Interactable = cash >= _cost;
            _targetGraphic.color = color;

            SetTextHint(_caption, cash, _cost);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if(string.Empty == _key)
                _key = WarriorId.Names[_id.Value];
        }
#endif
    }
}
