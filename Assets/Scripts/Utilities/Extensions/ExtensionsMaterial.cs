using UnityEngine;

public static class ExtensionsMaterial
{
    public static void SetTailing(this Material self, float value) => self.SetFloat("_Tiling", value);

    //public static void SetSeed(this Material self, float minInclusive, float maxInclusive) => self.SetFloat("_Seed", Random.Range(minInclusive, maxInclusive));
    //public static void SetDensity(this Material self, Vector2 value) => self.SetFloat("_Density", Random.Range(value.x, value.y));
    //public static void SetDensity(this Material self, float value) => self.SetFloat("_Density", value);
    
    //public static void SetBorder(this Material self, Vector2 value) => self.SetVector("_Border", new(value.x, value.y, 0f, 0f));
    //public static void SetColor(this Material self, Color value) => self.SetColor("_Color", value);
    //public static void SetBaseColorRandom(this Material self, Color value, float minInclusive, float maxInclusive) => self.SetColor("_BaseColor", value * Random.Range(minInclusive, maxInclusive));
    //public static void SetEmission(this Material self, Color value) => self.SetColor("_EmissionColor", value);
}
