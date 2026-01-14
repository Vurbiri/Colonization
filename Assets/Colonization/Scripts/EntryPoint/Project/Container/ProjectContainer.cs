using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.Storage;
using Vurbiri.Yandex;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class ProjectContainer : IContainer
	{
        private static ProjectContent s_projectContent;

        public static bool Valid
        { [Impl(256)] get => s_projectContent != null; }

        public static Settings Settings              
        { [Impl(256)] get => s_projectContent.settings; }

        public static GameSettings GameSettings      
        { [Impl(256)] get => s_projectContent.gameSettings; }

        public static Prices Prices                  
        { [Impl(256)] get => s_projectContent.prices; }

        public static HumansMaterials Materials      
        { [Impl(256)] get => s_projectContent.humansMaterials; }

        public static IStorageService StorageService 
        { [Impl(256)] get => s_projectContent.storageService; }

        public static YandexSDK YSDK                 
        { [Impl(256)] get => s_projectContent.ysdk; }

        public class UI
        {
            public static ProjectColors Colors       
            { [Impl(256)] get => s_projectContent.projectColors; }

            public static PlayerColors PlayerColors  
            { [Impl(256)] get => s_projectContent.playerColors; }
            public static PlayerNames PlayerNames    
            { [Impl(256)] get => s_projectContent.playerNames; }
            public static PlayerUINames PlayerColorNames
            { [Impl(256)] get => s_projectContent.playerUINames; }
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
