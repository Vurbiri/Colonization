using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization.Actors;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
	[CustomEditor(typeof(SFXFactoriesStorage), true), CanEditMultipleObjects]
	public class SFXFactoriesStorageEditor : Editor
	{
		private readonly List<Factory> _factories = new();
        private SFXFactoriesStorage _target;
        private SerializedProperty _serializedProperty;
        private Vector2 scrollPos;

        private void OnEnable()
		{
            _target = (SFXFactoriesStorage)target;
            _serializedProperty = serializedObject.FindProperty("_factories");

			UpdateFactories();
        }
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
            
            Space(0.5f);
			if (GUILayout.Button("Update"))
				UpdateFactories();
            
            scrollPos = BeginScrollView(scrollPos);
            {
                Space();
                for (int i = 0; i < _factories.Count; i++)
                    _factories[i].Draw();
            }
            EndScrollView();

            serializedObject.ApplyModifiedProperties();
		}

		private void UpdateFactories()
		{
            _target.Update_Ed();
            serializedObject.Update();

            _factories.Clear();
			_factories.Capacity = _serializedProperty.arraySize;

            for (int i = 0, j = 1; i < _serializedProperty.arraySize; i++, j++)
				_factories.Add(new(j, _serializedProperty.GetArrayElementAtIndex(i)));

        }

        #region Nested Factory
        private class Factory
		{
            private readonly int _nameIndex;
			private readonly SerializedProperty _property;
            private readonly Editor _editor;

            public Factory(int nameIndex, SerializedProperty property)
            {
                _nameIndex = nameIndex;
                _property = property;
                _editor = CreateEditor(property.objectReferenceValue);
			}

			public void Draw()
			{
                string name = _nameIndex.ToString("D2").Concat(" ", SFXFactoriesStorage.names_ed[_nameIndex]);
                if (_property.isExpanded = BeginFoldoutHeaderGroup(_property.isExpanded, name))
                {
                    BeginVertical(GUI.skin.box);
                    {
                        _editor.OnInspectorGUI();
                    }
                    EndVertical();
                }
                EndFoldoutHeaderGroup();
            }
        }
        #endregion
    }
}