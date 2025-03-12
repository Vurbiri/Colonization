//Assets\Colonization\Scripts\Characteristics\Buffs\Demon\DemonBuffs.cs
namespace Vurbiri.Colonization.Characteristics
{
    public class DemonBuffs
	{
		private readonly Perk _addMaxHP = new(ActorAbilityId.MaxHP, TypeModifierId.BasePercent, 5);
        private const int HP_LVL = 1;
        private readonly Perk _addHPPerTurn = new(ActorAbilityId.HPPerTurn, TypeModifierId.BasePercent, 5);
        private const int HPTURN_LVL= 3;
        private readonly Perk _addDefense = new(ActorAbilityId.Defense, TypeModifierId.BasePercent, 5);
        private const int DEF_LVL = 6;
        private readonly Perk _addAttack = new(ActorAbilityId.Attack, TypeModifierId.BasePercent, 5);
        private const int ATK_LVL = 7;
        private readonly Perk _addMaxAP = new(ActorAbilityId.Defense, TypeModifierId.Addition, 1);
    }
}
