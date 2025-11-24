using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class DemonAI
    {
        sealed private class FreeFinding : State
        {
            private Hexagon _targetHexagon;

            [Impl(256)] public FreeFinding(Actor.AI<DemonsAISettings, DemonId, DemonAIStateId> parent) : base(parent) { }

            public override bool TryEnter()
            {
                _targetHexagon = null;
                if (Status.isMove && ((Hexagon == Key.Zero && !GameContainer.Satan.CanEnterToGate) || (Status.isSiege && !IsInCombat && s_settings.chanceFreeFinding.Roll)))
                {
                    var hexagons = Hexagon.Neighbors; Hexagon hex;
                    foreach(var index in s_hexagonIndexes)
                    {
                        hex = hexagons[index];
                        if(hex.CanWarriorEnter && !hex.IsEnemyNear(PlayerId.Satan))
                        {
                            _targetHexagon = hex;
                            break;
                        }
                    }
                }
                return _targetHexagon != null;
            }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return Actor.Move_Cn(_targetHexagon);

                isContinue.Set(false);
                Exit();
                yield break;
            }

            public override void Dispose() => _targetHexagon = null;
        }
    }
}
