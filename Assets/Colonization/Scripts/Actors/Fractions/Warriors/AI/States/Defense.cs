using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class Defense : AIState
        {
            private bool _isBuff, _isBlock;
            
            public Defense(WarriorAI parent) : base(parent) { }

            public override bool TryEnter() 
            {
                return (Action.CanUseSkill(s_settings.defenseBuff[Actor.Id].skill) || Action.CanUseSpecSkill()) && IsEnemyComing();
            }

            public override void Dispose() { }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return GameContainer.CameraController.ToPositionControlled(Actor.Position);
                yield return Defense_Cn(true, true);

                isContinue.Set(false);
                Exit();
            }

        }
    }
}
