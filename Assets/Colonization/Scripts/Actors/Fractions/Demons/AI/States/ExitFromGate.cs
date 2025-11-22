using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class DemonAI
    {
        sealed private class ExitFromGate : State
        {
            private Hexagon _targetHexagon;

            [Impl(256)] public ExitFromGate(Actor.AI<DemonsAISettings, DemonId, DemonAIStateId> parent) : base(parent) { }

            public override bool TryEnter()
            {
                _targetHexagon = null;

                if (Status.isMove && IsInCombat && Hexagon == Key.Zero && !GameContainer.Players.Satan.CanEnterToGate)
                {
                    var hexagons = Hexagon.Neighbors;
                    foreach (int index in s_hexagonIndexes)
                    {
                        if (hexagons[index].IsEmpty)
                        {
                            _targetHexagon = hexagons[index];
                            break;
                        }
                    }
                }

                return _targetHexagon != null;
            }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return Actor.Move_Cn(_targetHexagon);

                isContinue.Set(true);
                Exit();
                yield break;
            }

            public override void Dispose() => _targetHexagon = null;
        }
    }
}
