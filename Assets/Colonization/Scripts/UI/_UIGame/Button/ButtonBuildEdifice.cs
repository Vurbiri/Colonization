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

        public void Init(Vector3 localPosition, IReadOnlyList<ACurrencies> edificePrices)
        {
            base.Init(localPosition);
            _edificePrices = edificePrices;
            _localization = SceneServices.Get<Language>();
        }

        public void Setup(bool isEnable, int edificeId, Color color, ACurrencies cash)
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
            _targetGraphic.color = color;

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
                _edificeView[i].keyHint = EdificeId.Names[i];
            }
        }
#endif
    }
}
