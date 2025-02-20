//Assets\Colonization\Scripts\Characteristics\Formulas.cs
namespace Vurbiri.Colonization.Characteristics
{
    public static class Formulas
	{
		public static int Damage(float damage, float defense)
		{
            return UnityEngine.Mathf.RoundToInt(damage * (1f - defense / (1f + defense + damage)));
		}
	}
}
