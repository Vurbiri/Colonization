using System.Runtime.CompilerServices;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
	public class ProjectContainer : IContainer
	{
        protected const MethodImplOptions Inline = MethodImplOptions.AggressiveInlining;

        private static ProjectContent s_projectContent;

        public static Settings Settings { [MethodImpl(Inline)] get => s_projectContent.settings; }
        public static GameSettings GameSettings { [MethodImpl(Inline)] get => s_projectContent.gameSettings; }

        public static Prices Prices { [MethodImpl(Inline)] get => s_projectContent.prices; }

        public static HumansMaterials Materials { [MethodImpl(Inline)] get => s_projectContent.humansMaterials; }

        public static IStorageService StorageService { [MethodImpl(Inline)] get => s_projectContent.storageService; }

        public static YandexSDK YSDK { [MethodImpl(Inline)] get => s_projectContent.ysdk; }

        public class UI
        {
            public static ProjectColors Colors { [MethodImpl(Inline)] get => s_projectContent.projectColors; }
            public static PlayerColors PlayerColors { [MethodImpl(Inline)] get => s_projectContent.playerColors; }
            public static PlayerNames PlayerNames { [MethodImpl(Inline)] get => s_projectContent.playerNames; }
        }

        public ProjectContainer() { }
        public ProjectContainer(ProjectContent content) => s_projectContent ??= content;

        public virtual void Dispose()
        {
            s_projectContent.Dispose();
            s_projectContent = null;
        }
    }
}
