namespace Vurbiri.Colonization
{
	public interface IPlayerController : System.IDisposable
	{
        public void OnLanding();
        public void OnEndLanding();
        public void OnPlay();
        public void OnEndTurn();
        public void OnStartTurn();
        public void OnProfit(Id<PlayerId> id, int hexId);
    }
}
