using UnityEngine;

namespace Vurbiri.Colonization
{
    public interface IPerkSettingsUI 
    {
        public int Id { get; }
        public int Type { get; }
        public int Level { get; }
        public int Position { get; }
        public Sprite Sprite { get; }
        public string KeyName { get; }
        public string KeyDescription { get; }
        public Id<TypeOperationId> TypeOperation { get; }
        public int PrevPerk { get; }
        public CurrenciesLite Cost { get; }
    }
}
