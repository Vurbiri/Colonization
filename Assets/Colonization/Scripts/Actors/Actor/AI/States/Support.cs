using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            sealed private class Support : Heal
            {
                private readonly List<Actor> _friends = new(3);

                [Impl(256)] public Support(AI<TSettings, TActorId, TStateId> parent) : base(parent) { }

                sealed public override bool TryEnter()
                {
                    bool isEnter = false;

                    _friends.Clear();
                    if (Settings.support && Status.nearFriends.NotEmpty)
                    {
                        var buffs = Settings.buffs;
                        List<Actor> friends = Status.nearFriends; Actor friend;
                        for (int i = 0; i < friends.Count; i++)
                        {
                            friend = friends[i];
                            if (buffs.CanUsed(Actor, friend))
                                _friends.Add(friend);
                        }
                        isEnter = TryHeal() || _friends.Count > 0;
                    }

                    return isEnter;
                }

                sealed public override IEnumerator Execution_Cn(Out<bool> isContinue)
                {
                    yield return Heal_Cn();

                    if (_friends.Count > 0)
                        yield return Settings.buffs.TryUse_Cn(Actor, _friends.Rand());

                    isContinue.Set(Actor.CurrentAP > 0);
                    Exit();
                    yield break;
                }

                sealed public override void Dispose()
                {
                    _friends.Clear();
                }
            }
        }
    }
}
