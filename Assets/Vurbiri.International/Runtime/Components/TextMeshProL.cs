using TMPro;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.International.UI
{
    [AddComponentMenu("Mesh/TextMeshPro - Text (Localization)")]
    public class TextMeshProL : TextMeshPro
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
            if (Application.isPlaying)
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
