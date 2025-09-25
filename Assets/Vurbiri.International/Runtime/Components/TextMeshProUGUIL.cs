using TMPro;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.International.UI
{
    [AddComponentMenu("UI/TextMeshPro - Text (UI Localization)", 12)]
    public class TextMeshProUGUIL : TextMeshProUGUI
    {
        [SerializeField] private FileIdAndKey _getText;

        private Subscription _subscribe;

        public void SetKey(FileId file, string key)
        {
            _getText = new(file, key);
            if (_subscribe == null)
                _subscribe = Localization.Instance.Subscribe(SetText);
            else
                text = Localization.Instance.GetText(_getText);
        }

        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            if(Application.isPlaying)
#endif
            _subscribe ??= Localization.Instance.Subscribe(SetText);
        }

        private void SetText(Localization localization) => text = localization.GetText(_getText);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _subscribe?.Dispose();
        }
    }
}
