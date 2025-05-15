//Assets\Colonization\Scripts\EntryPoint\Project\ProjectEntryPoint.cs
using Vurbiri.EntryPoint;
using Vurbiri.International;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class ProjectEntryPoint : AProjectEntryPoint
	{
        protected override ILoadingScreen Screen => GetComponent<ProjectInitialization>().Screen;
        protected override string LoadingDesc => Localization.Instance.GetText(Files.Main, "Loading");

        private void Start()
		{
            GetComponent<ProjectInitialization>().Init(_projectContainer, _loading);
        }
	}
}
