using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class Combat : AIState
        {
            public override int Id => WarriorAIStateId.Combat;

            public Combat(WarriorAI parent) : base(parent) { }

            public override bool TryEnter() => IsInCombat;

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                if (Status.nearFriends.NotEmpty)
                    yield return Settings.heal.TryUse_Cn(Actor, Status.nearFriends.Extract());

                yield return Settings.selfBuffs.TryUse_Cn(Actor);

                var enemy = Status.nearEnemies.Extract();
                yield return Settings.debuffs.TryUse_Cn(Actor, enemy);
                yield return Settings.attacks.TryUse_Cn(Actor, enemy);

                isContinue.Set(Actor.CurrentAP > 0);
                Exit();
            }

            public override void Dispose() { }
        }
    }
}
