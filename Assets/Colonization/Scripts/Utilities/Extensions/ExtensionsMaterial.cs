using UnityEngine;

public static class ExtensionsMaterial
{
    public static void SetTailing(this Material self, float value) => self.SetFloat("_Tiling", value);
}
