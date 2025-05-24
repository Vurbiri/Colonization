namespace Vurbiri.Colonization
{
	public interface IPlayerController : System.IDisposable
	{
        public void Init();
        public void Play();
        public void EndTurn();
        public void StartTurn();
        public void Profit(Id<PlayerId> id, int hexId);
    }
}
