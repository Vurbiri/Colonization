using Vurbiri.Colonization.EntryPoint;

namespace Vurbiri.Colonization
{
	public class MainMenuContainer : ProjectContainer
    {
        private static MainMenuContent s_content;

        public MainMenuContainer(MainMenuContent content)
        {
            s_content ??= content;
        }

        public override void Dispose()
        {
            s_content.Dispose();
            s_content = null;
        }
    }
}
