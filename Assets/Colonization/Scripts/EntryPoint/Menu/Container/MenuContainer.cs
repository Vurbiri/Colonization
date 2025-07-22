using Vurbiri.Colonization.EntryPoint;

namespace Vurbiri.Colonization
{
	public class MenuContainer : ProjectContainer
    {
        private static MenuContent s_content;

        public MenuContainer(MenuContent content) => s_content ??= content;

        public override void Dispose()
        {
            s_content.Dispose();
            s_content = null;
        }
    }
}
