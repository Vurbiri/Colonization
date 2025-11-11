using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class Escape : AIState
        {
            private Hexagon _target;

            [Impl(256)] public Escape(WarriorAI parent) : base(parent) { }

            public override bool TryEnter()
            {
                bool isEscape = Status.isMove;

                if (isEscape)
                {
                    Status.FindOwnedColoniesHex(Actor);



                    if (IsInCombat)
                    {

                    }


                    if (isEscape)
                    {
                        isEscape = (!Status.isGuard || Actor.CurrentHP < s_settings.minHPUnsiege) && EscapeChance(Status.nearTwo.force) && TryEscape(3, out _target);
                    }
                }

                return isEscape;
            }

            public override void Dispose() => _target = null;

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {

                yield return Move_Cn(_target);

                yield return Defense_Cn(true, true);

            }
        }
    }
}
