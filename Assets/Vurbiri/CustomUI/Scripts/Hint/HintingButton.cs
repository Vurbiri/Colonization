using UnityEngine;
using Vurbiri.Localization;

namespace Vurbiri.UI
{
    public class HintingButton : AHinting
    {
        [SerializeField] private string _key;

        public string Key { get => _key; set { _key = value; SetText(Language.Instance); } }

        private void Start() => Init();

        protected override void SetText(Language localization) => _text = localization.GetText(_file, _key);
    }
}
