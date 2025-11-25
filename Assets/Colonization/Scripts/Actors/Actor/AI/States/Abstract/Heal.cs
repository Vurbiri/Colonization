using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            protected abstract class Heal : State
            {
                protected const int BASE_HP = 105;
                private readonly WeightsList<Actor> _wounded = new(3);

                [Impl(256)] protected Heal(AI<TSettings, TActorId, TStateId> parent) : base(parent) { }

                protected bool CanHeal()
                {
                    if (!Settings.heal.IsValid) return false;

                    _wounded.Clear();
                    if (Status.nearFriends.NotEmpty)
                    {
                        var heal = Settings.heal;
                        List<Actor> friends = Status.nearFriends; Actor friend;
                        for (int i = 0; i < friends.Count; i++)
                        {
                            friend = friends[i];
                            if (heal.CanUsed(Actor, friend))
                                _wounded.Add(friend, BASE_HP - friend._HP.Percent);
                        }
                    }
                    return _wounded.Count > 0;
                }

                [Impl(256)] protected IEnumerator Heal_Cn()
                {
                    if (_wounded.Count > 0)
                    {
                        yield return Settings.heal.TryUse_Cn(Actor, _wounded.Roll);
                        _wounded.Clear();
                    }
                    yield break;
                }

                [Impl(256)] protected IEnumerator TryHeal_Cn()
                {
                    if (!Settings.heal.IsValid)
                        yield break;
                    
                    Status.nearFriends.Update(Actor);
                    if (CanHeal())
                    {
                        yield return Settings.heal.TryUse_Cn(Actor, _wounded.Roll);
                        _wounded.Clear();
                    }
                }
            }
        }
    }
}
