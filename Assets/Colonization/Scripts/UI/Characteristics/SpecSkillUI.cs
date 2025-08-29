namespace Vurbiri.Colonization.UI
{
    sealed public class SpecSkillUI : ASkillUI
    {
        public SpecSkillUI(ProjectColors colors, SeparatorEffectUI separator) : base(colors, separator, 0)
        {
            _textMain = "Spec Skill"; _textAP = "";
        }

        public override void Dispose() { }
    }
}
