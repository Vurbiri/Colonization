using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class Defense : AIState
        {
            private bool _isBuff, _isBlock;

            [Impl(256)] public Defense(WarriorAI parent) : base(parent) { }

            public override bool TryEnter()
            {
                _isBuff = _isBlock = false;
                if (IsEnemyComing)
                {
                    _isBuff  = s_settings.defenseBuff[Actor.Id].CanUsed(Action, Actor);
                    _isBlock = Action.CanUsedSpecSkill() && _parent._blockChance.Roll;
                }

                return _isBuff | _isBlock;
            }

            public override void Dispose() { }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return GameContainer.CameraController.ToPositionControlled(Actor.Position);

                if (_isBuff)
                    yield return s_settings.defenseBuff[Actor.Id].Use(Action);
                if (_isBlock && Action.CanUsedSpecSkill())
                    yield return Action.UseSpecSkill();

                isContinue.Set(false);
                Exit();
            }
        }
    }
}
