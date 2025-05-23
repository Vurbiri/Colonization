//Assets\Colonization\Scripts\Players\Player\Human.cs
using System;
using System.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using static Vurbiri.Colonization.Characteristics.HumanAbilityId;

namespace Vurbiri.Colonization
{
    public partial class Human : IPlayerController, IDisposable
    {
        #region Fields
        private readonly Coroutines _coroutines;

        private readonly Id<PlayerId> _id;
        private readonly bool _isPlayer;
        private readonly Currencies _resources;
        private readonly ExchangeRate _exchange;
        private readonly Prices _prices;

        private readonly PlayerScore _score;

        private readonly Edifices _edifices;
        private readonly Roads _roads;

        private readonly AbilitiesSet<HumanAbilityId> _abilities;
        private readonly Buffs _artefact;
        private readonly PerkTree _perks;

        private readonly WarriorsSpawner _spawner;
        private readonly ReactiveSet<Actor> _warriors = new(6);
        private readonly Unsubscription _unsubscriber;
        #endregion

        public ACurrenciesReactive Resources => _resources;
        public ExchangeRate Exchange => _exchange;

        public ReactiveSet<Actor> Warriors => _warriors;

        public ReactiveList<Crossroad> Shrines => _edifices.shrines;
        public ReactiveList<Crossroad> Ports => _edifices.ports;
        public ReactiveList<Crossroad> Colonies => _edifices.colonies;

        public Roads Roads => _roads;

        public PerkTree Perks => _perks;

        public Human(Id<PlayerId> playerId, HumanStorage storage, Players.Settings settings)
        {
            _id = playerId;
            _isPlayer = playerId == PlayerId.Player;
            _coroutines = SceneContainer.Get<Coroutines>();
            _score = SceneContainer.Get<Id<PlayerId>, PlayerScore >(playerId);

            var loadData = storage.LoadData;
            var visual = SceneContainer.Get<HumansMaterials>()[playerId];

            _perks = PerkTree.Create(settings, loadData);
            _abilities = settings.humanStates.Get(_perks);

            _roads = new(playerId, visual.color, settings.roadFactory, _coroutines);
            _prices = settings.prices;

            _resources = Currencies.Create(_abilities, _prices, loadData);
            _exchange = ExchangeRate.Create(_abilities, loadData);
            _artefact = Buffs.Create(settings.artefact.Settings, loadData);

            _spawner = new(new(playerId, _artefact, new(_perks)), settings.warriorPrefab, visual.materialWarriors, settings.actorsContainer);

            if (loadData.isLoaded)
            {
                _edifices = new(this, loadData.edifices, settings.crossroads);
                storage.PopulateRoads(_roads, settings.crossroads);

                Warrior warrior;
                for (int i = loadData.actors.Count - 1; i >= 0; i--)
                {
                    warrior = _spawner.Load(loadData.actors[i], settings.hexagons);
                    warrior.OnKilled.Add(ActorKill);
                    _warriors.Add(warrior);
                }
            }
            else
            {
                _edifices = new(_abilities);
            }

            bool instantGetValue = !loadData.isLoaded;
            storage.BindCurrencies(_resources, instantGetValue);
            storage.BindExchange(_exchange, instantGetValue);
            storage.BindPerks(_perks, instantGetValue);
            storage.BindRoads(_roads, instantGetValue);
            storage.BindArtefact(_artefact, instantGetValue);
            storage.BindEdifices(_edifices.edifices, instantGetValue);
            storage.BindActors(_warriors);

            storage.LoadData = null;

            settings.crossroads.BindEdifices(_edifices.edifices, instantGetValue);
        }

        public Ability GetAbility(Id<HumanAbilityId> id) => _abilities[id];

        public void Init()
        {

        }

        public void EndTurn()
        {
            int countBuffs = 0;
            CurrenciesLite profit = new();
            foreach (var warrior in _warriors)
            {
                if (warrior.IsMainProfit)
                    profit.Increment(warrior.Hexagon.SurfaceId);
                if (warrior.IsAdvProfit)
                    countBuffs++;

                warrior.StatesUpdate();
                warrior.IsPlayerTurn = false;
            }

            _resources.AddFrom(profit);
            _artefact.Next(countBuffs);

            _edifices.Interactable = false;
        }

        public void Profit(Id<PlayerId> id, int hexId)
        {
            if(_id == id)
                _resources.AddBlood(_edifices.ShrinePassiveProfit);

            if (hexId == CONST.GATE_ID)
            {
                _resources.AddBlood(_edifices.ShrineProfit);
                _resources.ClampMain();
                return;
            }

            if (_abilities.IsTrue(IsFreeGroundRes))
                _resources.AddFrom(Hexagons.FreeResources);

            _resources.AddFrom(_edifices.ProfitFromEdifices(hexId));
        }

        public void StartTurn()
        {
            foreach (var warrior in _warriors)
                warrior.EffectsUpdate();

            _exchange.Update();
        }

        public void Play()
        {
            _edifices.Interactable = _isPlayer;
            foreach (var warrior in _warriors)
                warrior.IsPlayerTurn = _isPlayer;
        }

        public ReactiveList<Crossroad> GetEdifices(Id<EdificeGroupId> id) => _edifices.edifices[id];

        public void BuyPerk(int typePerk, int idPerk)
        {
            if (_perks.TryAdd(typePerk, idPerk, out int cost))
                _resources.Add(CurrencyId.Blood, -cost);
        }

        #region Edifice
        public bool CanEdificeUpgrade(Crossroad crossroad) => _edifices.CanEdificeUpgrade(crossroad) && crossroad.CanUpgrade(_id);
        public void BuyEdificeUpgrade(Crossroad crossroad)
        {
            if (crossroad.BuyUpgrade(_id))
            {
                int edificeId = crossroad.Id.Value;
                _edifices.edifices[crossroad.GroupId].AddOrChange(crossroad);

                _resources.Pay(_prices.Edifices[edificeId]);
                _score.Build(edificeId);
            }
        }

        public bool CanWallBuild(Crossroad crossroad) => _abilities.IsTrue(IsWall) && crossroad.CanWallBuild(_id);
        public void BuyWall(Crossroad crossroad)
        {
            if (crossroad.BuyWall(_id, _abilities[WallDefence]))
            {
                _resources.Pay(_prices.Wall);
                _edifices.edifices[crossroad.GroupId].Signal(crossroad);
            }
        }
        #endregion

        #region Roads
        public bool CanRoadBuild(Crossroad crossroad) => _abilities.IsGreater(MaxRoads, _roads.Count) && crossroad.CanRoadBuild(_id);
        public void BuyRoad(Crossroad crossroad, Id<LinkId> linkId)
        {
            _resources.Pay(_prices.Road);
            _roads.BuildAndUnion(crossroad.GetLinkAndSetStart(linkId));
        }
        #endregion

        #region Warriors
        public bool CanAnyRecruiting(Crossroad crossroad)
        {
            return _abilities.IsGreater(MaxWarrior, _warriors.Count) && crossroad.CanRecruiting(_id);
        }
        public bool CanRecruiting(Id<WarriorId> id) => _abilities.IsTrue(id.ToState());

        public void Recruiting(Id<WarriorId> id, Crossroad crossroad) => _coroutines.Run(Recruiting_Cn(id, crossroad));
        public void Recruiting(Id<WarriorId> id, Hexagon hexagon)
        {
            _resources.Pay(_prices.Warriors[id.Value]);

            Warrior warrior = _spawner.Create(id, hexagon);
            warrior.OnKilled.Add(ActorKill);
            warrior.IsPlayerTurn = _isPlayer;
            
            _warriors.Add(warrior);
        }

        private IEnumerator Recruiting_Cn(Id<WarriorId> id, Crossroad crossroad)
        {
            WaitResult<Hexagon> result = crossroad.GetHexagonForRecruiting_Wait();
            yield return result;

            if (result.Value == null)
                yield break;

            Recruiting(id, result.Value);
        }
        #endregion

        public void Dispose()
        {
            _unsubscriber.Unsubscribe();
            _exchange.Dispose();
            _warriors.Dispose();
        }

        private void ActorKill(Id<PlayerId> target, int actorId)
        {
            if (target == PlayerId.Satan)
            {
                _score.DemonKill(actorId);
                _resources.AddBlood(actorId + 1);
            }
            else if (target != _id)
            {
                _score.WarriorKill(actorId);
            }
        }
    }
}
