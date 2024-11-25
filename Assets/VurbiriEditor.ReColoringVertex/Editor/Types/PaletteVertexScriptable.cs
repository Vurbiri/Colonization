//Assets\VurbiriEditor.ReColoringVertex\Editor\Types\PaletteVertexScriptable.cs
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VurbiriEditor.ReColoringVertex
{
    [CreateAssetMenu(fileName = "Palette_", menuName = "Vurbiri/Editor/Palette Vertex", order = 51)]
    internal class PaletteVertexScriptable : ScriptableObject, IEnumerable<VertexMaterial>
    {
        [SerializeField] private bool _isModify = false;
        [SerializeField] private Mesh _materialFromMesh;

        [SerializeField] private bool _isInvertSubMeshes = false;
        [SerializeField] private List<VertexMaterial> _vertexMaterials = new();
        [SerializeField] private bool _isEditName = true;

        public VertexMaterial this[int index] { get => _vertexMaterials[index]; set => _vertexMaterials[index] = value; }
        public int Count => _vertexMaterials.Count;

        public bool IsInvert => _isInvertSubMeshes;
        public bool IsEditName {get => _isEditName; set => _vertexMaterials.ForEach(v => v.isEditName = _isEditName = value); }
        public bool IsOpen {set => _vertexMaterials.ForEach(v => v.isOpen = value); }

        public void Init(string name, bool isInvert, bool isEditName, int count)
        {
            this.name = name;
            _isInvertSubMeshes = isInvert;
            _isEditName = isEditName;
            _vertexMaterials = new(count);
        }

        public void Add(VertexMaterial material) => _vertexMaterials.Add(material);

        public void Override(IEnumerable<VertexMaterial> materials)
        {
            if (materials != null)
                _vertexMaterials = new(materials);
        }

        public void Append(IEnumerable<VertexMaterial> materials)
        {
            if (materials == null)
                return;

            if (_vertexMaterials == null || _vertexMaterials.Count == 0)
            {
                _vertexMaterials = new(materials);
                return;
            }

            foreach (var material in materials)
                if (!FindMaterial(material))
                    _vertexMaterials.Add(material);

            #region Local: FindMaterial()
            //=================================
            bool FindMaterial(VertexMaterial material)
            {
                foreach (var mat in _vertexMaterials)
                    if (mat.ReplaceEquals(material))
                        return true;
                return false;
            }
            #endregion
        }

        public bool CheckMesh() => _isModify && _materialFromMesh != null && _materialFromMesh.colors32 != null && _materialFromMesh.colors32.Length > 0;

        public void OpenWindow()
        {
            var wnd = EditorWindow.GetWindow<PaletteFromMeshWindow>(true);

            wnd.EventOverride += Override;
            wnd.EventAppend += Append;

            wnd.CreateColorsData(_materialFromMesh);

            _isModify = false;
            _materialFromMesh = null;
        }

        public void Sort()
        {
            if (_vertexMaterials == null)
                return;

            _vertexMaterials.Sort();
        }

        public void NameFromColor()
        {
            if (_vertexMaterials == null)
                return;

            foreach (var mat in _vertexMaterials)
                mat.NameFromColor(mat.colorReplace);
        }

        public void ClearNames()
        {
            if (_vertexMaterials == null)
                return;

            foreach (var mat in _vertexMaterials)
                mat.name = string.Empty;
        }

        public IEnumerator<VertexMaterial> GetEnumerator() => _vertexMaterials?.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _vertexMaterials?.GetEnumerator();
    }
}
