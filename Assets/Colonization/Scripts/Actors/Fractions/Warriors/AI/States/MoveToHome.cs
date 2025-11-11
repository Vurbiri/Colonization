using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToHome : AIState
        {
            private Hexagon _targetHexagon;
            private Key _targetColony;

            [Impl(256)] public MoveToHome(WarriorAI parent) : base(parent) { }

            public override bool TryEnter()
            {
                _targetHexagon = null;

                if (Status.isMove & !(Status.isSiege | Status.isGuard))
                {
                    int distance = s_settings.maxDistanceHome;

                    if (TryGetEmptyColony(Colonies, ref distance, out Crossroad colony, out Hexagon target, Goals.CanGoHome))
                    {
                        _targetHexagon = target;
                        _targetColony = colony.Key;
                    }
                }
                return _targetHexagon != null && Goals.Home.Add(_targetColony);
            }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return Move_Cn(isContinue, 0, _targetHexagon, !_targetHexagon.CanWarriorEnter);
                if (!isContinue && IsEnemyComing)
                {
                    isContinue.Set(true);
                    Exit();
                }
            }

            public override void Dispose()
            {
                if(_targetHexagon != null)
                {
                    _targetHexagon = null;
                    Goals.Home.Remove(_targetColony);
                }
            }
        }
    }
}
