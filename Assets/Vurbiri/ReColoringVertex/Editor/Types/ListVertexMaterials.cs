using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor.ReColoringVertex
{
    internal class ListVertexMaterials : ScriptableObject, IEnumerable<VertexMaterial>
    {
        private const string NAME_PROPERTY = "_vertexMaterial";
        private const string LABEL_IS_EDIT = " EDIT", LABEL_FOLDOUT = "SubMesh ";

        [SerializeField] private List<VertexMaterial> _vertexMaterial = new();
        private readonly List<GroupVertex> _colorsVertexes = new();
        private SerializedProperty _property;
        private SerializedObject _this;

        private GUIContent _labelContent;
        private bool _isEdit = false;
        private Color _colorGUI;

        public bool IsEdit => _isEdit;

        public ListVertexMaterials Init(int id)
        {
            _labelContent = new($"{LABEL_FOLDOUT} {id}");

            _colorGUI = GUI.color;

            _this = new(this);
            _this.Update();
            _property = _this.FindProperty(NAME_PROPERTY);

            return this;
        }

        public VertexMaterial this[int index] { get => _vertexMaterial[index]; set => _vertexMaterial[index] = value; }
        public int Count => _colorsVertexes.Count;

        public void Draw(bool isEditName)
        {
            if (_colorsVertexes.Count != _vertexMaterial.Count)
                _vertexMaterial.Resize(_colorsVertexes.Count);

            for (int i = 0; i < _vertexMaterial.Count; i++)
                _vertexMaterial[i].Update(_colorsVertexes[i], isEditName, _isEdit);

            _this.Update();

            if (!(_isEdit = EditorGUILayout.ToggleLeft(LABEL_IS_EDIT, _isEdit)))
                GUI.color = Color.gray;
            EditorGUILayout.PropertyField(_property, _labelContent, true);
            GUI.color = _colorGUI;
            EditorGUILayout.Space();

            if (_colorsVertexes.Count == _property.arraySize)
                _this.ApplyModifiedProperties();
        }

        public void Add(Color colorMesh, int index)
        {
            _colorsVertexes.Add(new(colorMesh, index));
            _vertexMaterial.Add(new(colorMesh));

            _isEdit = _colorsVertexes.Count > 1;
        }

        public void Add(Color colorMesh, Vector2 uv, int index)
        {
            _colorsVertexes.Add(new(colorMesh, index));
            _vertexMaterial.Add(new(colorMesh, colorMesh, uv));

            _isEdit = _colorsVertexes.Count > 1;
        }

        public void Add(Color colorMesh, VertexMaterial material, int index)
        {
            _colorsVertexes.Add(new(colorMesh, index));
            _vertexMaterial.Add(new(colorMesh, material));

            _isEdit = _colorsVertexes.Count > 1;
        }

        public bool TryInsert(Color colorMesh, int vertex)
        {
            for(int i = 0; i < _colorsVertexes.Count; i++)
                if (_colorsVertexes[i].TryInsert(colorMesh, vertex))
                    return true;

            return false;
        }

        public void CopyValues(Color32[] colors, Vector2[] uvs)
        {
            for (int v = 0; v < _colorsVertexes.Count; v++)
                Set(_colorsVertexes[v], _vertexMaterial[v].colorReplace, _vertexMaterial[v].specular);

            #region Local: Set()
            //=================================
            void Set(GroupVertex group, Color32 colorReplace, Vector2 specular)
            {
                foreach (int i in group)
                {
                    colors[i] = colorReplace;
                    uvs[i] = specular;
                }
            }
            #endregion
        }

        public void CopyValues(Color32[] colors, Vector2[] uvs, Color32[] colorsSource, Vector2[] uvsSource)
        {
            for (int v = 0; v < _colorsVertexes.Count; v++)
                Set(_colorsVertexes[v]);

            #region Local: Set()
            //=================================
            void Set(GroupVertex group)
            {
                foreach (int i in group)
                {
                    colors[i] = colorsSource[i];
                    uvs[i] = uvsSource[i];
                }
            }
            #endregion
        }

        public IEnumerator<VertexMaterial> GetEnumerator() => _vertexMaterial.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _vertexMaterial.GetEnumerator();

        #region Nested: GameSave
        //***********************************
        private class GroupVertex : IEnumerable<int>
        {
            private Color _color;
            private readonly List<int> _indexesVertex;

            public GroupVertex(Color color, int index)
            {
                this._color = color;
                _indexesVertex = new()
                {
                    index
                };
            }

            public bool TryInsert(Color colorMesh, int vertex)
            {
                if(_color == colorMesh)
                {
                    _indexesVertex.Add(vertex);
                    return true;
                }

                return false;
            }

            public static implicit operator Color(GroupVertex value) => value._color;

            public IEnumerator<int> GetEnumerator() => _indexesVertex.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => _indexesVertex.GetEnumerator();
        }
        #endregion
    }
}
