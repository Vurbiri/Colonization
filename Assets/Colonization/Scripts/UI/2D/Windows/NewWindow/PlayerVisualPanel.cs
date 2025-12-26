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
		[SerializeField] private ColorInputField _inputColor;
		[SerializeField] private HintButton _colorReset;

		public (string, Color32) Input { [Impl(256)] get => new(_inputName.text, _inputColor.Color); }

		public void Init(PlayerNames names, PlayerColors colors, ColorWindow window)
		{
			_inputName.SetTextWithoutNotify(names[_id]);
			_inputName.onEndEdit.AddListener(OnEndNameEdit);
			_nameReset.AddListener(NameReset); _nameReset = null;

			_inputColor.Init(colors[_id], window);
			_colorReset.AddListener(ColorReset); _colorReset = null;
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

		private void ColorReset()
		{
			_inputColor.SetColor(ProjectContainer.UI.PlayerColors.GetDefault(_id));
		}

#if UNITY_EDITOR

		private RectTransform _panel;
		
		public void OnValidate(int id, MonoBehaviour parent)
		{
			_id = id;
			parent.SetChildren(ref _panel, $"PlayerVisualPanel_{id}");
			_panel.SetChildren(ref _inputName);
			_inputName.SetChildren(ref _nameReset);
			_panel.SetChildren(ref _inputColor);
			_inputColor.SetChildren(ref _colorReset);
		}
#endif
	}
}
