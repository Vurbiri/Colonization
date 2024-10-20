using UnityEngine;

public class PlayerVisual
{
    public readonly int id;
    public readonly Color color;
    public readonly Material materialLit;
    public readonly Material materialUnlit;
    public readonly Material materialWarriors;

    public PlayerVisual(int id, Color color, Material materialLit, Material materialUnlit, Material materialWarriors)
    {
        materialLit.color = color;
        materialUnlit.color = color;
        materialWarriors.color = color;

        this.id = id;
        this.color = color;
        this.materialLit = materialLit;
        this.materialUnlit = materialUnlit;
        this.materialWarriors = materialWarriors;
    }
}
