using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            protected abstract class MoveTo : Heal
            {
                protected const int DISTANCE_RATE = 10;

                protected Hexagon _targetHexagon;

                [Impl(256)] protected MoveTo(AI<TSettings, TActorId, TStateId> parent) : base(parent) { }

                protected IEnumerator Move_Cn(Out<bool> isContinue, int distance, bool isExit, bool isBuff, bool isBlock)
                {
                    isExit |= IsInCombat;
                    if (!isExit && Status.isMove)
                    {
                        isExit = !TryGetNextHexagon(Actor, _targetHexagon, out Hexagon next);
                        if (!isExit)
                        {
                            yield return Actor.Move_Cn(next);
                            isExit = _targetHexagon.Distance(next) == distance;
                            if (!isExit)
                            {
                                Status.EnemiesUpdate(Actor);
                                if (!(isExit = IsInCombat))
                                {
                                    yield return TryHeal_Cn();

                                    if ((isBuff | isBlock) && IsEnemyComing)
                                        yield return Settings.defense.TryUse_Cn(Actor, isBuff, isBlock);
                                }
                            }
                        }
                    }
                    isContinue.Set(isExit);
                    if (isExit) Exit();
                }
            }
        }
    }
}
