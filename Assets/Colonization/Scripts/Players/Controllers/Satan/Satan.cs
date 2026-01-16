using System;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public partial class Satan : Player, IReactive<Satan>
	{
		private const int DISPLAY_MAX_CURSE = 99;
		
		protected static readonly SatanAbilities s_parameters;

		protected readonly Spawner _spawner;
		protected readonly Artefact _artefact;
		private readonly SatanLeveling _leveling;
		private readonly VAction<Satan> _eventChanged = new();
		private int _curse, _maxCurse;

		public static int DefenseFromGate { [Impl(256)] get => s_parameters.gateDefense; }

		public int Level { [Impl(256)] get => _leveling.Level; }
		public int MaxLevel { [Impl(256)] get => _leveling.MaxLevel; }
		public int Curse { [Impl(256)] get => _curse * DISPLAY_MAX_CURSE / _maxCurse; }
		public int MaxCurse { [Impl(256)] get => DISPLAY_MAX_CURSE; }
		public float CursePercent { [Impl(256)] get => (float)_curse / _maxCurse; }

		static Satan() => s_parameters = SettingsFile.Load<SatanAbilities>();
		protected Satan(Settings settings, WaitAllWaits waitSpawn) : base(PlayerId.Satan, false)
		{
			var storage = GameContainer.Storage.Satan;
			var loadData = storage.LoadData; storage.LoadData = null;

			_curse = loadData.state.curse;
			_maxCurse = s_parameters.curse.maxBase + loadData.state.level * s_parameters.curse.perLevel;

			_leveling = new(settings.satanLeveling, loadData.state.level);
			_artefact = Artefact.Create(settings.artefact, loadData);

			_spawner = new(new(PlayerId.Satan, _leveling, _artefact), loadData.isLoaded ? loadData.state.spawn : s_parameters.potential.start);

			ActorsLoad(_spawner, loadData.actors, waitSpawn);

			storage.StateBind(this, !loadData.isLoaded);
			storage.BindArtefact(_artefact, !loadData.isLoaded);
			storage.BindActors(Actors);
		}

		public Subscription Subscribe(Action<Satan> action, bool instantGetValue) => _eventChanged.Add(action, this, instantGetValue);

		protected void AddCurse(int add)
		{
			if (add > 0)
			{
				_curse += add;

				var leveling = _leveling;
				var settings = s_parameters.curse;

				if (_curse >= _maxCurse)
				{
					if (leveling.Next())
						_maxCurse = settings.maxBase + leveling.Level * settings.perLevel;

					_curse -= _maxCurse;
					_spawner.AddPotential(1 + (leveling.Level / s_parameters.potential.levelRatio));
					GameContainer.Chaos.ForSatanLevelUP(leveling.Level / s_parameters.levelRatio);
				}

				_eventChanged.Invoke(this);
			}
		}

		[Impl(256)] public void Spawn(int demonId, Hexagon start) => _spawner.Create(demonId, start);
	}
}
