namespace Vurbiri.Colonization.Actors
{
    sealed public class WarriorsSettingsScriptable : ActorSettingsScriptable<WarriorId, WarriorSettings>
    {
        public override Id<ActorTypeId> TypeId => ActorTypeId.Warrior;
    }
}
