namespace Vurbiri.Colonization.UI
{
    using System.Collections.Generic;

    public static class CONST_UI_LNG_KEYS
	{
        public const string KEY_ATTACK = "Attack", KEY_DAMAGE = "Damage", KEY_AP = "AP";

		public static readonly IReadOnlyList<IReadOnlyList<string>> KEYS_DESK_EFFECTS = new string[][] {
																	new string[0], // MaxHP
                                                                    new string[]{ "RegenerationHPAddition"},
                                                                    new string[]{ "AttackAddition", "", "AttackPercent"},
                                                                    new string[]{ "DefenseAddition", "", "DefensePercent"},};
    }
}
