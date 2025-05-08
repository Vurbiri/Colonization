//Assets\Colonization\Scripts\Players\View\HumanMaterial.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class HumanMaterials
    {
        public readonly Material materialLit;
        public readonly Material materialUnlit;
        public readonly Material materialWarriors;
        public Color color;

        public HumanMaterials(Material materialLit, Material materialUnlit, Material materialWarriors)
        {
            this.materialLit = materialLit;
            this.materialUnlit = materialUnlit;
            this.materialWarriors = materialWarriors;
        }

        public void SetColor(Color color)
        {
            this.color = color;
            materialLit.color = color;
            materialUnlit.color = color;
            materialWarriors.color = color;
        }
    }
}
