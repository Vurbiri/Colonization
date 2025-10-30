using System.Collections.Generic;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    sealed public class Artefact : ABuffs<Buff>, IReactive<Artefact>
    {
        private int _level = 0;
        private readonly int[] _levels, _add;
        private readonly VAction<Artefact> _changeLevels = new();
        private readonly RandomIndex _rIndex;
        private readonly int _count;

        public int BuffsCount => _count;
        public int Level => _level;
        public int[] Levels => _levels;

        #region Constructors
        private Artefact(int maxLevel, List<BuffSettings> settings) : base(maxLevel)
        {
            _count = settings.Count;
            _rIndex = new(settings);

            _levels = new int[_count];
            _add = new int[_count];
            _buffs = new Buff[_count];
        }

        private Artefact(BuffsScriptable buffs) : this(buffs.MaxLevel, buffs.Settings)
        {
            var settings = buffs.Settings;
            for (int i = 0; i < _count; i++)
                _buffs[i] = new(_change, settings[i]);
        }

        private Artefact(BuffsScriptable buffs, int[] levels) : this(buffs.MaxLevel, buffs.Settings)
        {
            var settings = buffs.Settings;
            for (int i = 0; i < _count; i++)
            {
                _buffs[i] = new(_change, settings[i], _levels[i] = levels[i]);
                _level += levels[i];
            }
        }

        public static Artefact Create(BuffsScriptable buffs, APlayerLoadData loadData)
        {
            if(loadData.isLoaded & loadData.artefact != null) 
                return new(buffs, loadData.artefact);
            return new(buffs);
        }
        #endregion

        public void Next(int count)
        {
            count = System.Math.Min(count, _maxLevel - _level);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                    _add[_rIndex.Next]++;

                for (int i = 0, add; i < _count; i++)
                {
                    add = _add[i];
                    if (add > 0)
                    {
                        _add[i] = 0;
                        _buffs[i].Next(add);
                        _levels[i] += add;
                    }
                }
                _level += count;

                _changeLevels.Invoke(this);
            }
        }

        public Subscription Subscribe(System.Action<Artefact> action, bool instantGetValue = true)
        {
            return _changeLevels.Add(action, instantGetValue, this);
        }

        //=========== Nested ========================
        private class RandomIndex : WeightsList
        {
            public RandomIndex(List<BuffSettings> settings) : base(settings.Count)
            {
                for (int i = 0, count = _capacity - 1; i < count; i++)
                {
                    _weights[_count] = _weights[_count - 1] + settings[i].advance;
                    _count++;
                }
            }
        }
    }
}
