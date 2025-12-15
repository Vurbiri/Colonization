using System;
using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private class Counselors : IDisposable
        {
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
            [Impl(256)] public WaitResult<bool> GiftReceive(int giver, LiteCurrencies gift) => _diplomat.Receive(giver, gift);

            [Impl(256)] public void Update() => _diplomat.Update();

            public IEnumerator Execution_Cn()
            {
                foreach (var counsel in _counselors)
                    yield return counsel.Execution_Cn();

                yield return _commander.Execution_Cn();
            }

            public void Dispose() => _counselors.Dispose();
        }
    }
}
