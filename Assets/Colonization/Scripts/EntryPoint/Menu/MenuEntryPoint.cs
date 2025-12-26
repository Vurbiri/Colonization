using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.EntryPoint
{
	public class MenuEntryPoint : ASceneEntryPoint
	{
		[SerializeField] private SceneId _nextScene;
		[SerializeField] private FileIds _localizationFiles = new(false);
		[Space]
		[SerializeField] private MessageBoxColors _messageBoxColors;

		public override void Enter(Loading loading, Transition transition)
		{
			Log.Info("[MainMenu] Enter");

			transition.Setup(new Dummy(), _nextScene);
			
			Localization.Instance.SetFiles(_localizationFiles, true);
			MessageBox.SetColors(_messageBoxColors);

			Destroy(gameObject);
		}

		private class Dummy : System.IDisposable { public void Dispose() { Log.Info("[MainMenu] Exit"); } }


#if UNITY_EDITOR
		public void SetColors_Ed(SceneColorsEd colors)
		{
			_messageBoxColors.window = colors.panelBack;
			_messageBoxColors.text = colors.panelText;
			_messageBoxColors.button = colors.elements;
		}
#endif
	}
}
