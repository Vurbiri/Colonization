using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToColony : AIState
        {
            private Hexagon _targetHexagon;
            private Key _targetColony;

            [Impl(256)] public MoveToColony(WarriorAI parent) : base(parent) { }

            [Impl(256)] public override bool TryEnter() => Status.isMove && FindEmptyColony();

            [Impl(256)] public override IEnumerator Execution_Cn(Out<bool> isContinue) => Move_Cn(isContinue, 0, _targetHexagon);

            public override void Dispose()
            {
                if(_targetHexagon != null)
                {
                    _targetHexagon = null;
                    Goals.Defensed.Remove(_targetColony);
                }
            }

            private bool FindEmptyColony()
            {
                _targetHexagon = null;
                int distance = s_settings.maxDistanceEmpty;

                if ((!Status.isGuard || Status.minColonyGuard > 1) && TryGetEmptyColony(Colonies, Goals.Defensed, ref distance, out Crossroad colony, out Hexagon target))
                {
                    _targetHexagon = target;
                    _targetColony = colony.Key;
                }
                return _targetHexagon != null && Goals.Defensed.Add(_targetColony);
            }
        }
    }
}
