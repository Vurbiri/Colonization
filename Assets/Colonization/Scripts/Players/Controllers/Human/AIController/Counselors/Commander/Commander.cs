using System.Collections;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private class Commander : Counselor
        {
            private readonly RandomSequenceList<Warrior.AI> _warriorsAI = new(CONST.DEFAULT_MAX_WARRIOR);
            
            public Commander(AIController parent) : base(parent)
            {
                Human.Actors.Subscribe(OnActor);
            }

            public override IEnumerator Execution_Cn()
            {
                foreach (var warrior in _warriorsAI)
                    yield return StartCoroutine(warrior.Execution_Cn());
            }

            private void OnActor(Actor actor, TypeEvent type)
            {
                if (type == TypeEvent.Subscribe | type == TypeEvent.Add)
                    _warriorsAI.Add(Create(actor));
                else if (type == TypeEvent.Remove)
                    _warriorsAI.Remove(actor, Warrior.AI.Equals);

                // ====== Local ======
                [Impl(256)] static Warrior.AI Create(Actor actor) => actor.Id switch
                {
                    WarriorId.Militia => new MilitiaAI(actor),
                    WarriorId.Solder  => new SolderAI(actor),
                    WarriorId.Wizard  => new WizardAI(actor),
                    WarriorId.Warlock => new WarlockAI(actor),
                    WarriorId.Knight  => new KnightAI(actor),
                    _ => null
                };
            }
        }
    }
}
