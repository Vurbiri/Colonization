using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class MenuUISettings_Editor : MonoBehaviour, ICanvasElement
	{
		[StartEditor]
		[SerializeField, Button(nameof(UpdateVisual)), MinMax(1f, 2f)] private RefFloat _windowsPixelsPerUnit;
		[Space]
		[SerializeField, HideInInspector] private ColorSettingsScriptable _colorSettings;
		[SerializeField] private SceneColorsEd _colors;
		[Space]
		[SerializeField, HideInInspector] private CanvasHint _hint;
        [SerializeField, HideInInspector] private NewWindow _newWindow;
        [SerializeField, HideInInspector] private SettingsWindow _settingsWindow;
		[SerializeField, HideInInspector] private MainMenuManager _menu;
		[SerializeField, HideInInspector] private MenuEntryPoint _entryPoint;
		[EndEditor] public bool endEditor;

		private void Awake()
		{
			Destroy(gameObject);
		}

		public void UpdateVisual()
		{
			float windowsPixelsPerUnit = _windowsPixelsPerUnit;

			_hint.UpdateVisuals_Ed(_colors.hintBack, _colors.hintText);
            _newWindow.UpdateVisuals_Ed(windowsPixelsPerUnit, _colors);
            _settingsWindow.UpdateVisuals_Ed(windowsPixelsPerUnit, _colors);

			_menu.SetColors_Ed(_colors);
			_entryPoint.SetColors_Ed(_colors);
		}

		public void Rebuild(CanvasUpdate executing)
		{
			if (executing == CanvasUpdate.Prelayout && !Application.isPlaying)
				UpdateVisual();
		}
		public void LayoutComplete() { }
		public void GraphicUpdateComplete() { }
		public bool IsDestroyed() => this == null;

		private void OnValidate()
		{
			if (!Application.isPlaying)
			{
				EUtility.SetScriptable(ref _colorSettings);

				EUtility.SetObject(ref _hint);
                EUtility.SetObject(ref _newWindow);
                EUtility.SetObject(ref _settingsWindow);
				//EUtility.SetObject(ref _helpWindow);
				EUtility.SetObject(ref _menu);
				EUtility.SetObject(ref _entryPoint);

				CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);

                _colors = _colorSettings.menu;

                _windowsPixelsPerUnit = _colorSettings.windowsPixelsPerUnit;
            }
		}
	}
}
