using Vurbiri.EntryPoint;
using Vurbiri.International;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class ProjectEntryPoint : AProjectEntryPoint
	{
        protected override string LoadingDesc => Localization.Instance.GetText(LangFiles.Main, "Loading");

        private void Start()
		{
            var init = GetComponent<ProjectInitialization>();
            var content = new ProjectContent();

            Init(new ProjectContainer(content), init.Screen);
            init.Init(content, _loading, this);
        }
	}
}
