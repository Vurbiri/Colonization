using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            sealed private class EscapeSupport : State
            {
                private Hexagon _targetHexagon;

                [Impl(256)] public EscapeSupport(AI<TSettings, TActorId, TStateId> parent) : base(parent) { }

                sealed public override bool TryEnter()
                {
                    _targetHexagon = null;

                    if (Settings.support & Status.isMove & IsInCombat)
                    {
                        int distance = 2;
                        Status.nearFriends.GetNearSafeHexagon(Actor, ref distance, ref _targetHexagon);
                        Status.nighFriends.GetNearSafeHexagon(Actor, ref distance, ref _targetHexagon);

                        if(_targetHexagon == null && Status.nearFriends.Count == 0)
                            Status.nighFriends.GetNearHexagon(Actor, ref distance, ref _targetHexagon);

                        if (_targetHexagon != null && _targetHexagon.Distance(Hexagon) > 1)
                            TryGetNextHexagon(Actor, _targetHexagon, out _targetHexagon);
                    }

                    return _targetHexagon != null;
                }

                sealed public override IEnumerator Execution_Cn(Out<bool> isContinue)
                {
                    yield return Actor.Move_Cn(_targetHexagon);

                    isContinue.Set(true);
                    Exit();
                }

                sealed public override void Dispose() => _targetHexagon = null;
            }
        }
    }
}
