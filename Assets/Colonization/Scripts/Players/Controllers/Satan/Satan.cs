using System;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public partial class Satan : Player, IReactive<Satan>
	{
		protected static readonly SatanAbilities s_parameters;

		protected readonly Spawner _spawner;
		protected readonly Artefact _artefact;
		private readonly SatanLeveling _leveling;
		private readonly VAction<Satan> _eventChanged = new();
		private int _curse, _maxCurse;

		public static int DefenseFromGate { [Impl(256)] get => s_parameters.gateDefense; }

		public int Level { [Impl(256)] get => _leveling.Level; }
		public int MaxLevel { [Impl(256)] get => _leveling.MaxLevel; }
		public int Curse { [Impl(256)] get => _curse / s_parameters.maxCursePerLevel; }
		public int MaxCurse { [Impl(256)] get => _maxCurse / s_parameters.maxCursePerLevel; }
		public float CursePercent { [Impl(256)] get => (float)_curse / _maxCurse; }

		static Satan() => s_parameters = SettingsFile.Load<SatanAbilities>();
		protected Satan(Settings settings, WaitAllWaits waitSpawn) : base(PlayerId.Satan, false)
		{
			var storage = GameContainer.Storage.Satan;
			var loadData = storage.LoadData; storage.LoadData = null;

			_curse = loadData.state.curse;
			_maxCurse = s_parameters.maxCurseBase + loadData.state.level * s_parameters.maxCursePerLevel;

			_leveling = new(settings.satanLeveling, loadData.state.level);
			_artefact = Artefact.Create(settings.artefact, loadData);

			_spawner = new(new(PlayerId.Satan, _leveling, _artefact), loadData.state.spawn);

			ActorsLoad(_spawner, loadData.actors, waitSpawn);

			storage.StateBind(this, !loadData.isLoaded);
			storage.BindArtefact(_artefact, !loadData.isLoaded);
			storage.BindActors(Actors);
		}

		public Subscription Subscribe(Action<Satan> action, bool instantGetValue) => _eventChanged.Add(action, this, instantGetValue);

		protected void AddCurse(int add)
		{
			if(add <= 0) return;
			
			_curse += add;
			if (_curse >= _maxCurse)
			{
				if (_leveling.Next())
					_maxCurse = s_parameters.maxCurseBase + _leveling.Level * s_parameters.maxCursePerLevel;

				_curse -= _maxCurse;
				_spawner.AddPotential(1 + (_leveling.Level / s_parameters.potentialFromLvlRatio));
				GameContainer.Chaos.ForSatanLevelUP(_leveling.Level);
			}

			_eventChanged.Invoke(this);
		}

		[Impl(256)] public void Spawn(int demonId, Hexagon start) => _spawner.Create(demonId, start);
	}
}
