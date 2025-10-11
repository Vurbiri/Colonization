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

            private readonly List<Plan> _plans = new();
            private readonly MainCurrencies _profitWeights = new();
            private Plan _currentPlan = Plan.Empty;

            static Builder()
            {
                s_settings = SettingsFile.Load<BuilderSettings>();
                s_settings.profitWeight = -s_settings.profitWeight;
            }

            public Builder(AIController parent) : base(parent) { }

            public override IEnumerator Planning_Cn()
            {
                Log.Info($"===== {PlayerId.PositiveNames_Ed[Id]} ======");
                if (_currentPlan.Done || !_currentPlan.IsValid)
                    yield return CreatePlan_Cn();
            }

            public override IEnumerator Execution_Cn()
            {
                Log.Info(_currentPlan);
                yield return _currentPlan.Execution_Cn();
            }

            public IEnumerator BuildFirstPort_Cn()
            {
                Crossroad port = GameContainer.Crossroads.GetRandomPort();
                yield return GameContainer.CameraController.ToPositionControlled(port);
                yield return Human.BuildPort(port).signal;
            }

            private IEnumerator CreatePlan_Cn()
            {
                Log.Info("CreatePlan");
                List<Plan> plans = new() { Plan.Empty };

                SetProfitWeight();
                yield return null;

                if (Colonies.Count > 0)
                {
                    Upgrade.Create(this, plans, Colonies);
                    Upgrade.Create(this, plans, Ports);
                    yield return null;

                    WallBuild.Create(this, plans);
                    PortBuild.Create(this, plans);
                    yield return null;
                }

                yield return LandBuild.Create(this, plans);

                _currentPlan = GetPlan(plans);

                yield break;

                #region Local
                // ======================================
                [Impl(256)] void SetProfitWeight()
                {
                    _profitWeights.Clear();
                    var colonies = Colonies;
                    for (int i = 0; i < colonies.Count; i++)
                        colonies[i].AddNetProfit(_profitWeights);

                    _profitWeights.Normalize(s_settings.profitWeight);
                }
                // ======================================
                [Impl(256)] Plan GetPlan(List<Plan> plans)
                {
                    if(plans.Count == 1) return plans[0];

                    int weight = UnityEngine.Random.Range(0, plans[^1].Weight);
                    int min = 0, max = plans.Count, current;
                    while (true)
                    {
                        current = min + max >> 1;
                        if (plans[current] <= weight)
                            min = current;
                        else if (plans[current - 1] > weight)
                            max = current;
                        else
                            return plans[current];
                    }
                }
                // ======================================
                #endregion
            }

            [Impl(256)] private int GetCostWeight(ReadOnlyMainCurrencies cost) => Resources.Lack(cost) * s_settings.costWeight;
            [Impl(256)] private static int GetEdificeWeight(int id) => s_settings.edificeWeight[id];

            [Impl(256)] private int GetProfitWeight(List<Hexagon> hexagons)
            {
                int weight = 0;
                for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                    weight += _profitWeights[hexagons[i].GetProfit()];
                return weight;
            }
            private int GetFirstProfitWeight(List<Hexagon> hexagons)
            {
                MainCurrencies profitId = new();
                int weight = 0, profit;
                for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                {
                    profit = hexagons[i].GetProfit();
                    profitId.Increment(profit);
                    weight += _profitWeights[profit];
                }
                
                return weight - s_settings.penaltyPerHex ^ profitId.MaxValue;
            }
        }
    }
}
