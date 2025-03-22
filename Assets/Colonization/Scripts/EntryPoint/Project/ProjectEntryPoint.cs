//Assets\Colonization\Scripts\EntryPoint\Project\ProjectEntryPoint.cs
using System.Collections;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class ProjectEntryPoint : AProjectEntryPoint
	{
		private IEnumerator Start()
		{
            return GetComponent<ProjectInitialization>().Init_Cn(_servicesContainer, _dataContainer, _loadingScreen);
        }
	}
}
