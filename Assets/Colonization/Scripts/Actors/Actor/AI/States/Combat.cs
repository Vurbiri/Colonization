using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            sealed private class Combat : Heal
            {
                private readonly WeightsList<Actor> _enemies = new(3);

                [Impl(256)] public Combat(AI<TSettings, TActorId, TStateId> parent) : base(parent) { }

                sealed public override bool TryEnter()
                {
                    bool isEnter = false;
                    
                    _enemies.Clear();
                    if (IsInCombat)
                    {
                        var attacks = Settings.attacks;
                        List<Actor> enemies = Status.nearEnemies; Actor enemy;
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            enemy = enemies[i];
                            if (attacks.CanUsed(Actor, enemy))
                                _enemies.Add(enemy, BASE_HP - enemy.PercentHP);
                        }

                        var debuffs = Settings.debuffs;
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            enemy = enemies[i];
                            if (!_enemies.Contains(enemy) && debuffs.CanUsed(Actor, enemy))
                                _enemies.Add(enemy, enemy.PercentHP);
                        }

                        isEnter = TryHeal() || _enemies.Count > 0;
                    }

                    return isEnter;
                }

                sealed public override IEnumerator Execution_Cn(Out<bool> isContinue)
                {
                    yield return Heal_Cn();

                    yield return Settings.selfBuffs.TryUse_Cn(Actor);

                    if (_enemies.Count > 0)
                    {
                        var enemy = _enemies.Roll;
                        yield return Settings.debuffs.TryUse_Cn(Actor, enemy);
                        yield return Settings.attacks.TryUse_Cn(Actor, enemy);
                    }

                    if (OwnerId != PlayerId.Satan && Actor._AP == 0 && Actor._zealCharge && Actor.CurrentForce < Status.nearEnemies.Force)
                        yield return Actor.Zeal_Cn();

                    isContinue.Set(Actor._AP > 0);
                    Exit();
                    yield break;
                }

                sealed public override void Dispose()
                {
                    _enemies.Clear();
                }
            }
        }
    }
}
