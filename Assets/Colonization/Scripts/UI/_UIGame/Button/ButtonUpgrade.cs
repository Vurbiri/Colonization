using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Localization;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(CmButton))]
    public class ButtonUpgrade : AHinting
    {
        [Space]
        [SerializeField] private string _keyType = "Shrine";
        [SerializeField, Multiline] private string _format = "<align=\"center\">{0}\r\n";
        [Space]
        [SerializeField] private Image _buttonIcon;
        [Space]
        [SerializeField] private IdArray<EdificeId, View> _edificeView;

        private CmButton _button;
        private Language _localization;

        public CmButton Button => _button;

        public override void Init()
        {
            base.Init();
            _button = _thisSelectable as CmButton;
            _localization = Language.Instance;
        }

        public void SetupHint(int edificeId)
        {
            View view = _edificeView[edificeId];

            _buttonIcon.sprite = view.sprite;
            _keyType = view.key;

            _text = GetTextFormat(_localization);
        }

        public void AddListener(UnityEngine.Events.UnityAction action) => _button.onClick.AddListener(action);

        protected override void SetText(Language localization) => _text = GetTextFormat(localization);

        private string GetTextFormat(Language localization) => string.Format(_format, localization.GetText(_file, _keyType));

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

        #region Nested: Profile
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
