using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization
{
	[System.Serializable]
	public class PlayerVisualPanel : System.IDisposable
	{
		[SerializeField, ReadOnly] private Id<PlayerId> _id;
		[Space]
		[SerializeField] private TMP_InputField _inputName;

		public void Init(PlayerNames players)
		{
			_inputName.SetTextWithoutNotify(players[_id]);
            _inputName.onEndEdit.AddListener(SetCustomName);
        }

		private void SetCustomName(string name)
		{
            _inputName.SetTextWithoutNotify(ProjectContainer.UI.PlayerNames.SetCustomName(_id, name));
        }

		public void Dispose()
		{
            _inputName.onEndEdit.RemoveListener(SetCustomName);
        }

#if UNITY_EDITOR

        private RectTransform _panel;
		
		public void OnValidate(int id, MonoBehaviour parent)
		{
			_id = id;
            parent.SetChildren(ref _panel, $"PlayerVisualPanel_{id}");
			_panel.SetChildren(ref _inputName);
		}
#endif
    }
}
