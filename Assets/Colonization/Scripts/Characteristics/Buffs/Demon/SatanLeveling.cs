using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public class SatanLeveling : ABuffs<DemonBuff>
    {
        private int _level;

        public int Level { [Impl(256)] get => _level; }
        
        public SatanLeveling(BuffsScriptable buffs, int level) : base(buffs.MaxLevel)
        {
            _level = level;

            var settings = buffs.Settings;
            int count = settings.Count;
            _buffs = new DemonBuff[count];

            for (int i = 0; i < count; i++)
                _buffs[i] = new(_change, settings[i], level);
        }

        public bool Next()
        {
            bool result = _level < _maxLevel;
            if (result)
            {
                _level++;
                for (int i = _buffs.Length - 1; i >= 0; i--)
                    _buffs[i].Next(_level);
            }

            return result;
        }
    }
}
