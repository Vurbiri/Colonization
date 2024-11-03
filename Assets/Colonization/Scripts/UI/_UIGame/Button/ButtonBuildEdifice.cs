using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Collections;
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

        public void Init(Vector3 localPosition, Color color, IReadOnlyList<ACurrencies> edificePrices, UnityEngine.Events.UnityAction action)
        {
            base.Init(localPosition, color, action);
            _edificePrices = edificePrices;
            _localization = SceneServices.Get<Language>();
        }

        public void Setup(bool isEnable, int edificeId, ACurrencies cash)
        {
            if (!isEnable)
            {
                _thisGO.SetActive(false);
                return;
            }

            ButtonView view = _edificeView[edificeId];
            ACurrencies cost = _edificePrices[edificeId];

            _button.Interactable = cash >= cost;
            _buttonIcon.sprite = view.sprite;

            SetTextHint(_localization.GetText(_file, view.keyHint), cash, cost);

            _thisGO.SetActive(true);
        }

        #region Nested: ButtonView
        //*******************************************************
        [System.Serializable]
        private class ButtonView
        {
            public Sprite sprite;
            public string keyHint;
        }
        #endregion


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            for (int i = 0; i < EdificeId.Count; i++)
            {
                if(string.IsNullOrEmpty(_edificeView[i].keyHint))
                _edificeView[i].keyHint = EdificeId.GetName(i);
            }
        }
#endif
    }
}
