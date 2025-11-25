using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            sealed private class Healing : State
            {
                private const int BASE_HP = 105;
                private readonly WeightsList<Actor> _wounded = new(3);

                [Impl(256)] public Healing(AI<TSettings, TActorId, TStateId> parent) : base(parent) { }

                public override bool TryEnter()
                {
                    if (!Settings.heal.IsValid) return false;

                    _wounded.Clear(); 
                    if (!IsInCombat && Actor._AP > 0)
                    {
                        var hexagons = Hexagon.Neighbors; var heal = Settings.heal;
                        for (int i = 0; i < HEX.SIDES; i++)
                            if (hexagons[i].TryGetFriend(OwnerId, out Actor friend) && heal.CanUsed(Actor, friend))
                                _wounded.Add(friend, BASE_HP - friend._HP.Percent);
                    }
                    return _wounded.Count > 0;
                }

                public override IEnumerator Execution_Cn(Out<bool> isContinue)
                {
                    yield return Settings.heal.TryUse_Cn(Actor, _wounded.Roll);
                    _wounded.Clear();

                    isContinue.Set(true);
                    Exit();
                }

                public override void Dispose() { }
            }
        }
    }
}
