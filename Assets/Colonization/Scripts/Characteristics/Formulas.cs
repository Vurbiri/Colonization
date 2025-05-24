namespace Vurbiri.Colonization.Characteristics
{
    public static class Formulas
	{
		public static int Damage(double damage, double defense)
		{
            return (int)System.Math.Round(damage * (1.0 - defense / (defense + damage)));
		}
	}
}
