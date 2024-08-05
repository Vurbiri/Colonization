using UnityEngine;

namespace VurbiriEditor.ReColoringVertex
{
    [System.Serializable]
    internal class VertexMaterial
    {
        public string name;
        public Color colorMesh;
        public Color colorReplace;
        public Vector2 specular;

        public bool isInfoMode  = true;
        public bool isEditValue = true;
        public bool isEditName = true;
        public bool isOpen = true;

        public VertexMaterial Material { get => new(this); set => SetMaterial(value); }

        public VertexMaterial()
        {
            name = string.Empty;
            colorMesh = Color.black;
            colorReplace = Color.white;
            specular = Vector2.zero;
            isInfoMode = true;
            isEditValue = true;
            isEditName = true;
            isOpen = true;
        }

        public VertexMaterial(VertexMaterial material)
        {
            name = material.name;
            colorMesh = Color.black;
            colorReplace = material.colorReplace;
            specular = material.specular;
            isInfoMode = true;
            isEditValue = true;
            isEditName = material.isEditName;
            isOpen = true;
        }

        public VertexMaterial(Color colorMesh, VertexMaterial material)
        {
            NameFromColor(colorMesh);
            SetMaterial(material);
            this.colorMesh = colorMesh;
            isInfoMode = false;
        }

        public VertexMaterial(Color colorMesh)
        {
            NameFromColor(colorMesh);
            this.colorMesh = colorMesh;
            colorReplace = Color.white;
            specular = Vector2.zero;
            isInfoMode = false;
            isOpen = isEditValue;
        }

        public VertexMaterial(Color colorReplace, Vector2 specular)
        {
            NameFromColor(colorReplace);
            colorMesh = Color.black;
            this.colorReplace = colorReplace;
            this.specular = specular;
            isInfoMode = true;
            isEditValue = true;
            isEditName = false;
            isOpen = true;
        }

        public void Reset(bool isEditName)
        {
            colorMesh = Color.black;
            this.isEditName = isEditName;
            isEditValue = true;
            isInfoMode = true;
        }

        public void Update(Color colorMesh, bool isEditName, bool isEditValue, bool isInfoMode)
        {
            this.colorMesh = colorMesh;
            this.isEditName = isEditName;
            this.isEditValue = isEditValue;
            this.isInfoMode = isInfoMode;
        }

        public bool ReplaceEquals(VertexMaterial material) => colorReplace == material.colorReplace && specular == material.specular;

        public void NameFromColor(Color color)
        {
            Color32 color32 = color;
            name = $"[{color32.r}, {color32.g}, {color32.b}]";
        }

        private void SetMaterial(VertexMaterial material)
        {
            name = string.IsNullOrEmpty(material.name) ? name : material.name;
            colorReplace = material.colorReplace;
            specular = material.specular;
            isEditName = material.isEditName;
            isOpen = true;
        }
    }
}
