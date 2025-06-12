namespace Vurbiri.Colonization.Characteristics
{
    sealed public class DemonLeveling : ABuffs<DemonBuff>
    {
        public DemonLeveling(BuffsScriptable buffs, int level) : base(buffs.MaxLevel)
        {
            var settings = buffs.Settings;
            int count = settings.Count;
            _buffs = new DemonBuff[count];

            for (int i = 0; i < count; i++)
                _buffs[i] = new(_subscriber, settings[i], level);
        }

        public void Next(int level)
        {
            if (level <= _maxLevel)
                for (int i = _buffs.Length - 1; i >= 0; i--)
                    _buffs[i].Next(level);
        }
    }
}
