//Assets\Colonization\Scripts\CONSTS\CONST_UI_LNG_KEYS.cs
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.UI
{
    public static class CONST_UI_LNG_KEYS
	{
        public const Files FILE = Files.Actors;

        public const string ON_TARGET = "OnTarget", ON_SELF = "OnSelf";
        public const string AP_KEY = "AP", BLOCK_KEY = "Block", BLOCK_DESK_KEY = "DefenseTemp";
        public const string REFLECT_PLUS = "ReflectPlus", REFLECT_MINUS = "ReflectMinus";

        public static readonly string[] DESK_EFFECTS_KEYS =
        { "Damage", "DamagePierce", "Healing", "MaxHPTemp", "HPPerTurnTemp", "AttackTemp", "DefenseTemp", "CurrentHPPerm", "CurrentHPOfMaxPerm", "CurrentAPPerm", "IsMovePerm" };

        public static readonly string[] KEYS_NAME_SKILLS = { "Attack", "MagicAttack", "Sweep", "Combo", "Heal" };

        public const string PRESENT = "%", PLUS = "+", MINUS = "-";
    }
}
