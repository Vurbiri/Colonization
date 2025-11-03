using Vurbiri.Collections;
using static Vurbiri.Colonization.ActorAbilityId;

namespace Vurbiri.Colonization
{
    public static class Formulas
	{
		public static int Damage(double damage, double defense)
		{
            return (int)System.Math.Round(damage * (1.0 - defense / (defense + damage)));
		}

        public static int ActorForce(IdArray<ActorAbilityId, int> abilities)
        {
            return abilities[MaxHP] * abilities[Defense] * (abilities[Attack] + abilities[Pierce] << 1) * abilities[MaxAP] >> SHIFT_ABILITY;
        }
    }
}
