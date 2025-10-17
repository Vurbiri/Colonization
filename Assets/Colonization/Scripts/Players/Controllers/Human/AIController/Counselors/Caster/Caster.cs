using System.Collections;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private partial class Caster : Counselor
        {
            private static readonly CasterSettings s_settings;
            private static readonly ReadOnlyIdArray<AbilityTypeId, ReadOnlyArray<int>> s_weights;

            static Caster()
            {
                s_settings = SettingsFile.Load<CasterSettings>();
                s_weights = new(s_settings.weightsEconomic, s_settings.weightsMilitary);
                s_settings.weightsEconomic = null; s_settings.weightsMilitary = null;
            }

            public Caster(AIController parent) : base(parent)
            {
            }

            public override IEnumerator Init_Cn()
            {
                throw new System.NotImplementedException();
            }

            public override IEnumerator Execution_Cn()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
