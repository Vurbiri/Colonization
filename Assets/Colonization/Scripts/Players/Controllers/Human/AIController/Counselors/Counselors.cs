using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private class Counselors
        {
            private readonly WaitSignal _waitExecution = new();
            private readonly RandomSequence<Counselor> _counselors;
            private readonly Diplomat _diplomat;
            private readonly Builder _builder;

            private readonly Commander _commander;

            public Counselors(AIController parent)
            {
                _counselors = new( _diplomat = new(parent), _builder = new(parent), new Scientist(parent), new Caster(parent), new Recruiter(parent));
                _commander = new(parent.Actors);
            }

            [Impl(256)] public IEnumerator Landing_Cn() => _builder.Landing_Cn();
            [Impl(256)] public WaitResult<bool> GiftReceive(int giver, MainCurrencies gift) => _diplomat.Receive(giver, gift);

            [Impl(256)] public void Update() => _diplomat.Update();

            public WaitSignal Execution_Wait()
            {
                StartCoroutine(Execution_Cn());
                return _waitExecution.Restart();
            }

            private IEnumerator Execution_Cn()
            {
                foreach (var counsel in _counselors)
                    yield return StartCoroutine(counsel.Execution_Cn());

                yield return StartCoroutine(_commander.Execution_Cn());

                _waitExecution.Send();
            }

            [Impl(256)] private Coroutine StartCoroutine(IEnumerator routine) => GameContainer.Shared.StartCoroutine(routine);
        }
    }
}
