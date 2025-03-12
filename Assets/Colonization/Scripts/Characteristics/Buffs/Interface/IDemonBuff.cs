//Assets\Colonization\Scripts\Characteristics\Buffs\Interface\IDemonBuff.cs
namespace Vurbiri.Colonization.Characteristics
{
    public interface IDemonBuff : IBuff
    {
        public void Next(int level);
    }
}
