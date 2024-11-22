//Assets\Colonization\Scripts\Characteristics\Perk\Interface\IPerkSettingsUI.cs
namespace Vurbiri.Colonization.UI
{
    using Characteristics;
    using UnityEngine;

    public interface IPerkSettingsUI 
    {
        public int Id { get; }
        public int Type { get; }
        public int Level { get; }
        public int Position { get; }
        public Sprite Sprite { get; }
        public string KeyDescription { get; }
        public Id<TypeModifierId> TypeModifier { get; }
        public int PrevPerk { get; }
        public CurrenciesLite Cost { get; }
    }
}
