using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
	public interface IPlayerController : System.IDisposable
	{
        public Id<PlayerId> Id { get; }
        public bool IsPerson { get; }

        ReadOnlyReactiveSet<Actor> Actors { get; }

        public void ActorKill(Id<ActorTypeId> type, int id);

        public void OnLanding();
        public void OnEndLanding();
        public void OnPlay();
        public void OnEndTurn();
        public void OnStartTurn();
        public void OnProfit(Id<PlayerId> id, int hexId);
    }
}
