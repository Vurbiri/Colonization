using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Localization;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(CmButton))]
    public class ButtonBuildEdifice : AButtonBuild
    {
        [Space]
        [SerializeField] private Image _buttonIcon;
        [Space]
        [SerializeField] private IdArray<EdificeId, ButtonView> _edificeView;

        private Language _localization;
        private IReadOnlyList<ACurrencies> _edificePrices;

        public void Init(IReadOnlyList<ACurrencies> edificePrices)
        {
            base.Init();
            _edificePrices = edificePrices;
            _localization = SceneServices.Get<Language>();
        }

        public void SetupHint(int edificeId, ACurrencies cash)
        {
            ButtonView view = _edificeView[edificeId];

            _buttonIcon.sprite = view.sprite;
            SetTextHint(_localization.GetText(_file, view.keyHint), cash, _edificePrices[edificeId]);
        }
        

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            for (int i = 0; i < EdificeId.Count; i++)
            {
                if(string.IsNullOrEmpty(_edificeView[i].keyHint))
                _edificeView[i].keyHint = EdificeId.Names[i];
            }
        }
#endif
    }
}
