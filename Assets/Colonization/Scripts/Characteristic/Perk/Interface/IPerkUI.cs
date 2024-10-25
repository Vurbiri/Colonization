namespace Vurbiri.Colonization
{
    public interface IPerkUI 
    {
        public int Id { get; }
        public int Level { get; }
        public string KeyName { get; }
        public string KeyDescription { get; }
        public CurrenciesLite Cost { get; }
    }
}
