//Assets\Colonization\Scripts\Players\View\PlayerVisual.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class PlayerVisual
    {
        public Color color;
        public Material materialLit;
        public Material materialUnlit;
        public Material materialWarriors;

        public PlayerVisual(Color color)
        {
            this.color = color;
        }

        public PlayerVisual(Color color, Material materialLit, Material materialUnlit, Material materialWarriors)
        {
            materialLit.color = color;
            materialUnlit.color = color;
            materialWarriors.color = color;

            this.color = color;
            this.materialLit = materialLit;
            this.materialUnlit = materialUnlit;
            this.materialWarriors = materialWarriors;
        }
    }
}
