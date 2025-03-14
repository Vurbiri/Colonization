//Assets\Colonization\Scripts\Characteristics\Buffs\Demon\DemonBuffs.cs
using System.Collections.Generic;

namespace Vurbiri.Colonization.Characteristics
{
    public class DemonBuffs : ABuffs<DemonBuff>
	{
        public DemonBuffs(IReadOnlyList<DemonBuffSettings> settings)
        {
            _count = settings.Count;
            _buffs = new DemonBuff[_count];
            for (int i = 0; i < _count; i++)
                _buffs[i] = new(_subscriber, settings[i]);
        }

        public DemonBuffs(IReadOnlyList<DemonBuffSettings> settings, int level)
        {
            _count = settings.Count;
            _buffs = new DemonBuff[_count];
            for (int i = 0; i < _count; i++)
                _buffs[i] = new(_subscriber, settings[i], level);
        }

        public void Next(int level)
        {
            for (int i = 0; i < _count; i++)
                _buffs[i].Next(level);
        }
    }
}
