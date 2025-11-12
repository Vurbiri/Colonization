using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class FindResources : AIState
        {
            private readonly ReadOnlyCurrencies _resources;
            private readonly List<Hexagon> _hexagons = new();
            private readonly List<Id<CurrencyId>> _minResources = new();
            private int _minResCount;

            public override int Id => WarriorAIStateId.FindResources;

            [Impl(256)] public FindResources(WarriorAI parent) : base(parent) 
            {
                _resources = GameContainer.Players.Humans[OwnerId].Resources;
            }

            public override bool TryEnter() => Status.isMove && !(IsInCombat || IsEnemyComing);

            public override void Dispose() { }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                _minResCount = _resources[Actor.Hexagon.GetProfit()];

                if (Status.isGuard | Status.isSiege)
                    SetColoniesHexagon();
                else
                    AddHexagons(Actor.Hexagon.Key, Actor.Hexagon.Neighbors);

                if(_hexagons.Count > 0)
                {
                    SetMinResources();
                    RemoveHexagons();
                    if (_hexagons.Count > 0)
                        yield return Move_Cn(_hexagons.Rand());
                }

                _hexagons.Clear();
                _minResources.Clear();

                isContinue.Set(false);
                Exit();

                yield break;
            }

            private void SetColoniesHexagon()
            {
                Key current = Actor.Hexagon.Key;
                var crossroads = Actor.Hexagon.Crossroads;
                for (int i = 0; i < HEX.VERTICES; ++i)
                    if (crossroads[i].TryGetOwnerColony(out Id<PlayerId> playerId) && (playerId == OwnerId || GameContainer.Diplomacy.IsEnemy(playerId, OwnerId)))
                        AddHexagons(current, crossroads[i].Hexagons);
            }

            private void AddHexagons(Key current, ReadOnlyArray<Hexagon> hexagons)
            {
                Hexagon hexagon;
                for (int i = 0; i < hexagons.Count; ++i)
                {
                    hexagon = hexagons[i];
                    if (hexagon.CanWarriorEnter && hexagon.Distance(current) == 1 && !hexagon.IsEnemyNear(OwnerId))
                    {
                        _hexagons.Add(hexagon);
                        _minResCount = System.Math.Min(_minResCount, _resources[hexagon.GetProfit()]);
                    }
                }
            }

            [Impl(256)] private void SetMinResources()
            {
                for(int i = 0; i < CurrencyId.MainCount; ++i)
                    if (_resources[i] == _minResCount)
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
