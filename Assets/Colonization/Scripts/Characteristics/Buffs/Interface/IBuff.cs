//Assets\Colonization\Scripts\Characteristics\Buffs\Interface\IBuff.cs
using System;

namespace Vurbiri.Colonization.Characteristics
{
    public interface IBuff
	{
        public int Apply(Func<IPerk, int> func);
	}
}
