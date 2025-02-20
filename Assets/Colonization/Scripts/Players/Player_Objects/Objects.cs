//Assets\Colonization\Scripts\Players\Player_Objects\Objects.cs
using System;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class Player
    {
        protected partial class Objects : IDisposable
        {
            protected readonly PricesScriptable _prices;
            protected readonly WarriorsSpawner _spawner;

            public readonly Id<PlayerId> id;
            public readonly Currencies resources;
            public readonly Edifices edifices;
            public readonly Roads roads;
            public readonly AbilitiesSet<PlayerAbilityId> abilities;
            public readonly ListReactiveItems<Actor> warriors = new();
            public readonly HashSet<int> perks;

            public Objects(int playerId, int currentPlayerId, bool isLoad, PlayerData data, Players.Settings settings)
            {
                id = playerId;

                PlayerVisual visual = SceneData.Get<PlayersVisual>()[playerId];

                abilities = settings.states;
                roads = settings.roadsFactory.Create().Init(playerId, visual.color);
                _prices = settings.prices;
                _spawner = new(playerId, settings.warriorPrefab, visual.materialWarriors, settings.actorsContainer);

                if (isLoad)
                {
                    PlayerLoadData loadData = data.ToLoadData(currentPlayerId);
                    Crossroads crossroads = SceneObjects.Get<Crossroads>();
                    Land land = SceneObjects.Get<Land>();

                    resources = new(loadData.resources, abilities[PlayerAbilityId.MaxMainResources], abilities[PlayerAbilityId.MaxBlood]);
                    edifices = new(playerId, loadData.edifices, crossroads);
                    roads.Restoration(loadData.roads, crossroads);

                    int count = loadData.warriors.Length;
                    for (int i = 0; i < count; i++)
                        warriors.Add(_spawner.Load(loadData.warriors[i], land));

                    //_perks = new(data.Perks);
                }
                else
                {
                    resources = new(_prices.PlayersDefault, abilities[PlayerAbilityId.MaxMainResources], abilities[PlayerAbilityId.MaxBlood]);
                    edifices = new();
                    perks = new();
                }

                data.CurrenciesBind(resources, !isLoad);
                data.EdificesBind(edifices.values);
                data.RoadsBind(roads, !isLoad);
                data.WarriorsBind(warriors);
            }

            public void Dispose()
            {
                resources.Dispose();
                edifices.Dispose();
                for(int i = warriors.Count -1; i >= 0; i--)
                    warriors[i].Dispose();
            }
        }
    }
}
