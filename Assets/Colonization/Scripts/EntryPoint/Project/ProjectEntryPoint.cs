//Assets\Colonization\Scripts\EntryPoint\Project\ProjectEntryPoint.cs
using System.Collections;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class ProjectEntryPoint : AProjectEntryPoint
	{
        protected override ILoadingScreen Screen => LoadingScreen.Instance;
        protected override string LoadingDesc => Localization.Instance.GetText(Files.Main, "Loading");

        private IEnumerator Start()
		{
            return GetComponent<ProjectInitialization>().Init_Cn(_projectContainer, _loadingScreen);
        }
	}
}
