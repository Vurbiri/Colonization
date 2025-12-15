using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using static Vurbiri.Colonization.Actor;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class FindResources : State
        {
            private readonly List<Hexagon> _hexagons = new();
            private readonly List<Id<CurrencyId>> _minResources = new();
            private int _minResCount;

            [Impl(256)] public FindResources(AI<WarriorsAISettings, WarriorId, WarriorAIStateId> parent) : base(parent) { }

            public override bool TryEnter() => Status.isMove && !(IsInCombat || IsEnemyComing);

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                ReadOnlyCurrencies resources = GameContainer.Humans[OwnerId].Resources;

                _hexagons.Add(Hexagon);
                _minResCount = resources[Hexagon.GetProfit()];

                if (Status.isSiege || (Status.isGuard && !s_settings.chanceFreeFinding.Roll))
                    SetColoniesHexagon(resources);
                else
                    AddHexagons(Hexagon.Key, Hexagon.Neighbors, resources);

                SetMinResources(resources);
                RemoveHexagons();

                var target = _hexagons.Rand();
                if (target != Hexagon)
                    yield return Actor.Move_Cn(target);

                _hexagons.Clear();
                _minResources.Clear();

                isContinue.Set(false);
                Exit();
            }

            public override void Dispose() { }

            private void SetColoniesHexagon(ReadOnlyCurrencies resources)
            {
                Key current = Hexagon.Key;
                var crossroads = Hexagon.Crossroads;
                for (int i = 0; i < HEX.VERTICES; ++i)
                    if (crossroads[i].TryGetOwnerColony(out Id<PlayerId> playerId) && (playerId == OwnerId || GameContainer.Diplomacy.IsEnemy(playerId, OwnerId)))
                        AddHexagons(current, crossroads[i].Hexagons, resources);
            }

            private void AddHexagons(Key current, ReadOnlyArray<Hexagon> hexagons, ReadOnlyCurrencies resources)
            {
                Hexagon hexagon;
                for (int i = 0; i < hexagons.Count; ++i)
                {
                    hexagon = hexagons[i];
                    if (hexagon.CanWarriorEnter && hexagon.Distance(current) == 1 && !hexagon.IsEnemyNear(OwnerId))
                    {
                        _hexagons.Add(hexagon);
                        _minResCount = System.Math.Min(_minResCount, resources[hexagon.GetProfit()]);
                    }
                }
            }

            [Impl(256)] private void SetMinResources(ReadOnlyCurrencies resources)
            {
                for(int i = 0; i < CurrencyId.Count; ++i)
                    if (resources[i] == _minResCount)
                        _minResources.Add(i);
            }

            [Impl(256)] private void RemoveHexagons()
            {
                int index = _hexagons.Count;
                while (index --> 0)
                    if (!_minResources.Contains(_hexagons[index].GetProfit()))
                        _hexagons.RemoveAt(index);
            }
        }
    }
}
