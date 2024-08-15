using UnityEngine;

public class PlayerVisual
{
    public readonly int id;
    public readonly Color color;
    public readonly Material materialLit;
    public readonly Material materialUnlit;

    public PlayerVisual(int id, Color color, Material materialLit, Material materialUnlit)
    {
        materialLit.color = color;
        materialUnlit.color = color;

        this.id = id;
        this.color = color;
        this.materialLit = materialLit;
        this.materialUnlit = materialUnlit;
    }
}
