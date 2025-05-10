//Assets\Colonization\Scripts\EntryPoint\Project\ProjectEntryPoint.cs
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class ProjectEntryPoint : AProjectEntryPoint
	{
        protected override ILoadingScreen Screen => LoadingScreen.Instance;
        protected override string LoadingDesc => Localization.Instance.GetText(Files.Main, "Loading");

        private void Start()
		{
            GetComponent<ProjectInitialization>().Init(_projectContainer, _loading, LoadingScreen.Instance);
        }
	}
}
