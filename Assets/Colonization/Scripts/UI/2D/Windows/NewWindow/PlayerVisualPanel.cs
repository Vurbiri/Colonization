using TMPro;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.UI
{
	[System.Serializable]
	public class PlayerVisualPanel : System.IDisposable
	{
		[SerializeField, ReadOnly] private Id<PlayerId> _id;
		[Space]
		[SerializeField] private TMP_InputField _inputName;
        [SerializeField] private HintButton _nameReset;

        public string Name { [Impl(256)] get => _inputName.text; }

        public void Init(PlayerNames players)
		{
			_inputName.SetTextWithoutNotify(players[_id]);
            _inputName.onEndEdit.AddListener(OnEndNameEdit);

            _nameReset.AddListener(NameReset); _nameReset = null;
        }

        public void Dispose()
		{
            _inputName.onEndEdit.RemoveListener(OnEndNameEdit);
        }

        private void OnEndNameEdit(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = ProjectContainer.UI.PlayerNames[_id];

            if (_inputName.text != name)
                _inputName.SetTextWithoutNotify(name);
        }

        private void NameReset()
        {
            string name = ProjectContainer.UI.PlayerNames.GetDefault(_id);
            if (_inputName.text != name)
                _inputName.SetTextWithoutNotify(name);
        }

#if UNITY_EDITOR

        private RectTransform _panel;
		
		public void OnValidate(int id, MonoBehaviour parent)
		{
			_id = id;
            parent.SetChildren(ref _panel, $"PlayerVisualPanel_{id}");
			_panel.SetChildren(ref _inputName);
            _panel.SetChildren(ref _nameReset, "NameReset");
        }
#endif
    }
}
