namespace Vurbiri.Colonization.UI
{
    sealed public class RoadsPanel : ASinglyPanel<CurrentMax>
    {
        public void Init()
        {
            var person = GameContainer.Players.Person;
            _widget.Init(person.Roads.Count, person.GetAbility(HumanAbilityId.MaxRoad));

            Destroy(this);
        }
    }
}
