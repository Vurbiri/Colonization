using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private partial class Builder : Counselor
        {
            private static readonly BuilderSettings s_settings;
            
            private readonly MainCurrencies _profitWeights = new();
            private Plan _currentPlan = Plan.Empty;

            static Builder()
            {
                s_settings = SettingsFile.Load<BuilderSettings>();
                s_settings.profitWeight = -s_settings.profitWeight;
                s_settings.penaltyPerHex = -s_settings.penaltyPerHex;
            }

            public Builder(AIController parent) : base(parent) { }

            public override IEnumerator Init_Cn()
            {
                var port = GameContainer.Crossroads.GetRandomPort();
                yield return GameContainer.CameraController.ToPositionControlled(port);
                yield return Human.BuildPort(port).signal;
                yield return s_waitRealtime.Restart();
            }

            public override IEnumerator Execution_Cn()
            {
                if (_currentPlan.Done || !_currentPlan.IsValid)
                    yield return CreatePlan_Cn();

                Log.Info(_currentPlan);
                yield return _currentPlan.Execution_Cn();
            }

            private IEnumerator CreatePlan_Cn()
            {
                Plans plans = new();

                if (Colonies.Count > 0)
                {
                    SetProfitWeight();
                    yield return null;

                    Upgrade.Create(this, plans, Colonies);
                    Upgrade.Create(this, plans, Ports);
                    yield return null;

                    WallBuild.Create(this, plans);
                    PortBuild.Create(this, plans);
                    yield return null;
                }

                yield return LandBuild.Create_Cn(this, plans);

                _currentPlan = plans.Value;

                yield break;

                // Local
                [Impl(256)] void SetProfitWeight()
                {
                    _profitWeights.Clear();
                    var colonies = Colonies;
                    for (int i = 0; i < colonies.Count; i++)
                        colonies[i].AddNetProfit(_profitWeights);

                    _profitWeights.Normalize(s_settings.profitWeight);
                }
            }

            [Impl(256)] private int GetCostWeight(ReadOnlyMainCurrencies cost) => Resources.Deficit(cost) * s_settings.costWeight;
            [Impl(256)] private static int GetEdificeWeight(int id) => s_settings.edificeWeight[id];

            private int GetProfitWeight(List<Hexagon> hexagons)
            {
                int weight = 0;
                for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                    weight += _profitWeights[hexagons[i].GetProfit()];
                return weight;
            }

            private int GetColonyWeight(Crossroad crossroad, int roadCount) => GetProfitWeight(crossroad.Hexagons) + GetRoadWeight(roadCount);
            private int GetFirstColonyWeight(Crossroad crossroad, int roadCount) => s_settings.penaltyPerHex * crossroad.MaxRepeatProfit + GetRoadWeight(roadCount);

            [Impl(256)] private static int GetRoadWeight(int roadCount) => -MathI.Pow(s_settings.penaltyPerRoad, roadCount);


            // Nested Class
            private class Plans : WeightsList<Plan>
            {
                public Plans() : base(Plan.Empty) {}
            }
        }
    }
}
