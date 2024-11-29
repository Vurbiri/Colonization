//Assets\Colonization\Scripts\SFX\Spell\SpellSFXId.cs
namespace Vurbiri.Colonization.SFX
{
    public class SpellSFXId : AIdType<SpellSFXId>
    {
        public const int WarriorPositive = 0;
        public const int WarriorNegative = 1;


        static SpellSFXId() => RunConstructor();
        private SpellSFXId() { }
    }
}
