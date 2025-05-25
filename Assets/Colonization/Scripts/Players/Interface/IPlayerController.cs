namespace Vurbiri.Colonization
{
	public interface IPlayerController : System.IDisposable
	{
        public void OnInit();
        public void OnPlay();
        public void OnEndTurn();
        public void OnStartTurn();
        public void OnProfit(Id<PlayerId> id, int hexId);
    }
}
