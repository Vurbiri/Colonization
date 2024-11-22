//Assets\Colonization\Scripts\Players\View\PlayerVisual.cs
using UnityEngine;

public class PlayerVisual
{
    public readonly Color color;
    public readonly Material materialLit;
    public readonly Material materialUnlit;
    public readonly Material materialWarriors;

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
