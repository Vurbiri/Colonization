using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization.Characteristics;

namespace VurbiriEditor.Colonization.Characteristics
{
	public abstract class APerksWindow<T> : EditorWindow where T : APerksEditor<T>
    {
        [SerializeField] private PerksScriptable _perks;

        private Editor _editor;

        protected static readonly Vector2 s_sizeWindow = new(375f, 800f);

        public void CreateGUI()
        {
            EUtility.CheckScriptable(ref _perks, "Perks", "Assets/Colonization/Settings/Characteristics");

            rootVisualElement.Add(APerksEditor<T>.CreateEditorAndBind(_perks, out _editor));
        }

        private void OnDisable()
        {
            DestroyImmediate(_editor);
        }
    }
}
