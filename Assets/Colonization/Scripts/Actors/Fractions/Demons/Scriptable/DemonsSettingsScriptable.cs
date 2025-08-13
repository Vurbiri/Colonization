namespace Vurbiri.Colonization.Actors
{
    sealed public class DemonsSettingsScriptable : ActorSettingsScriptable<DemonId, DemonSettings>
    {
        public override Id<ActorTypeId> TypeId => ActorTypeId.Demon;
    }
}
