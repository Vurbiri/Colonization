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
        [SerializeField] private IdArray<EdificeId, View> _edificeView;

        private Language _localization;

        public override void Init()
        {
            base.Init();
            _localization = Language.Instance;
        }

        public void SetupHint(int edificeId, ACurrencies cash, ACurrencies cost)
        {
            View view = _edificeView[edificeId];

            _buttonIcon.sprite = view.sprite;
            SetTextHint(_localization.GetText(_file, view.key), cash, cost);
        }
        

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            for (int i = 0; i < EdificeId.Count; i++)
            {
                if(string.IsNullOrEmpty(_edificeView[i].key))
                _edificeView[i].key = EdificeId.Names[i];
            }
        }
#endif

        #region Nested: View
        //*******************************************************
        [System.Serializable]
        private class View
        {
            public Sprite sprite;
            public string key;
        }
        #endregion
    }
}
