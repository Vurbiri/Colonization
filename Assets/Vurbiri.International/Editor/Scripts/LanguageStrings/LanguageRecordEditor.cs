using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace VurbiriEditor.International
{
	[CustomEditor(typeof(LanguageRecord), true)]
	public class LanguageRecordEditor : AEditorGetVE<LanguageRecordEditor>
	{
		[SerializeField] private VisualTreeAsset _treeAssetList;
		[SerializeField] private VisualTreeAsset _treeAssetItem;

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = _treeAssetList.CloneTree();

			var list = root.Q<ListView>("Values");
			list.makeItem = _treeAssetItem.CloneTree;

			return root;
		}
	}
}
