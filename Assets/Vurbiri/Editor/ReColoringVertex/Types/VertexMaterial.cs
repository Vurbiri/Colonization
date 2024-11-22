//Assets\Vurbiri\Editor\ReColoringVertex\Types\VertexMaterial.cs
using System;
using UnityEngine;

namespace VurbiriEditor.ReColoringVertex
{
    [System.Serializable]
    internal class VertexMaterial : IComparable<VertexMaterial>
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

        public VertexMaterial(Color colorMesh, Color colorReplace, Vector2 specular)
        {
            NameFromColor(colorReplace);
            this.colorMesh = colorMesh;
            this.colorReplace = colorReplace;
            this.specular = specular;
            isInfoMode = false;
            isEditValue = true;
            isEditName = false;
            isOpen = true;
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

        public void Update(Color colorMesh, bool isEditName, bool isEditValue)
        {
            this.colorMesh = colorMesh;
            this.isEditName = isEditName;
            this.isEditValue = isEditValue;
            this.isInfoMode = false;
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

        public int CompareTo(VertexMaterial vm)
        {
            float[] self = new float[3];
            float[] other = new float[3];

            Color.RGBToHSV(colorReplace, out self[0], out self[1], out self[2]);
            Color.RGBToHSV(vm.colorReplace, out other[0], out other[1], out other[2]);

            for (int i = 0; i < 3; i++)
                if (!Mathf.Approximately(self[i], other[i])) return self[i].CompareTo(other[i]);

            for (int i = 0; i < 2; i ++)
                if (!Mathf.Approximately(specular[i], vm.specular[i])) return specular[i].CompareTo(vm.specular[i]);
            
            return 0;
        }
    }
}
