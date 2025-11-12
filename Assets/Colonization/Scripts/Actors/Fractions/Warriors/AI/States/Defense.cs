using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class Defense : AIState
        {
            private bool _isBuff, _isBlock;

            public override int Id => WarriorAIStateId.Defense;

            [Impl(256)] public Defense(WarriorAI parent) : base(parent) { }

            public override bool TryEnter()
            {
                _isBuff = _isBlock = false;
                if (IsEnemyComing)
                {
                    _isBuff  = Settings.defenseBuff.CanUsed(Action, Actor);
                    _isBlock = Action.CanUsedSpecSkill() && Settings.specChance.Roll;
                }

                return _isBuff | _isBlock;
            }

            public override void Dispose() { }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return GameContainer.CameraController.ToPositionControlled(Actor.Position);

                if (_isBuff)
                    yield return Settings.defenseBuff.Use(Action);
                if (_isBlock && Action.CanUsedSpecSkill())
                    yield return Action.UseSpecSkill();

                isContinue.Set(false);
                Exit();
            }
        }
    }
}
