using Vurbiri.EntryPoint;
using Vurbiri.International;

namespace Vurbiri.Colonization.EntryPoint
{
	sealed public class ProjectEntryPoint : AProjectEntryPoint
	{
		[UnityEngine.SerializeField] private FileIdAndKey _loadingDesc;
		
		private void Start()
		{
			var init = GetComponent<ProjectInitialization>();
			var content = new ProjectContent();

			Init(new ProjectContainer(content), init.Screen);
			init.Init(content, _loading, this);
		}

		protected override string LoadingDesc() => Localization.Instance.GetText(_loadingDesc);
	}
}
