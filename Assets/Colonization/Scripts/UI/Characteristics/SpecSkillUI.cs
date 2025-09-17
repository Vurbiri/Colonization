namespace Vurbiri.Colonization.UI
{
    sealed public class SpecSkillUI : ASkillUI
    {
        public SpecSkillUI(SeparatorEffectUI separator) : base(separator, 0)
        {
            _textMain = "Spec Skill"; _textAP = "";
        }

        public override void Dispose() { }
    }
}
