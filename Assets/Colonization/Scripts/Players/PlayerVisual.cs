using UnityEngine;

public class PlayerVisual
{
    public readonly int id;
    public readonly Color color;
    public readonly Material material;

    public PlayerVisual(int id, Color color, Material material)
    {
        material.color = color;

        this.id = id;
        this.color = color;
        this.material = material;
    }
}
