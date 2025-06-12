using System.Collections.Generic;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class Artefact : ABuffs<Buff>, IReactive<IReadOnlyList<int>>
    {
        private readonly RInt _level = new(0);
        private readonly int[] _levels;
        private readonly Subscription<IReadOnlyList<int>> _changeLevels = new();
        private readonly RandomIndex _rIndex;

        public RInt Level => _level;

        #region Constructors
        private Artefact(int maxLevel, List<BuffSettings> settings) : base(maxLevel)
        {
            _rIndex = new(settings);
            _levels = new int[settings.Count];
            _buffs = new Buff[settings.Count];
        }

        private Artefact(BuffsScriptable buffs) : this(buffs.MaxLevel, buffs.Settings)
        {
            var settings = buffs.Settings;
            for (int i = settings.Count - 1; i >= 0; i--)
                _buffs[i] = new(_subscriber, settings[i]);
        }

        private Artefact(BuffsScriptable buffs, int[] levels) : this(buffs.MaxLevel, buffs.Settings)
        {
            var settings = buffs.Settings;
            for (int i = settings.Count - 1; i >= 0; i--)
            {
                _buffs[i] = new(_subscriber, settings[i], _levels[i] = levels[i]);
                _level.Add(levels[i]);
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
            count = System.Math.Min(count, _maxLevel - _level.Value);

            if (count <= 0) return;
            
            for (int i = 0, index; i < count; i++)
            {
                index = _rIndex.Next();
                _buffs[index].Next();
                _levels[index]++;
            }

            _changeLevels.Invoke(_levels);
            _level.Add(count);
        }

        public Unsubscription Subscribe(System.Action<IReadOnlyList<int>> action, bool instantGetValue = true)
        {
            return _changeLevels.Add(action, instantGetValue, _levels);
        }

        #region Nested class RandomIndex
        private class RandomIndex
        {
            private readonly int[] _weights;
            private readonly int _maxWeight;
            private readonly int _count;

            public RandomIndex(List<BuffSettings> settings)
            {
                _count = settings.Count;
                _weights = new int[_count];
                _weights[0] = settings[0].advance;
                for (int i = 1; i < _count; i++)
                    _weights[i] = _weights[i - 1] + settings[i].advance;

                _maxWeight = _weights[^1];
            }

            public int Next()
            {
                int weight = UnityEngine.Random.Range(0, _maxWeight);
                for (int i = 0; i < _count; i++)
                    if(weight <  _weights[i])
                        return i;
                return -1;
            }
        }
        #endregion
    }
}
