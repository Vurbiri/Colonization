using Vurbiri.Colonization.Storage;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.Yandex;

namespace Vurbiri.Colonization.EntryPoint
{
    public class ProjectContent : IContainerContent
    {
        public Settings settings;
        public GameSettings gameSettings;

        public ProjectColors projectColors;
        public PlayerColors playerColors;
        public PlayerNames playerNames;
        public PlayerUINames playerUINames;
        public HumansMaterials humansMaterials;
        public Prices prices;

        public IStorageService storageService;
        public ProjectStorage projectStorage;
        public YandexSDK ysdk;

        public void Dispose() 
        {
            playerUINames.Dispose();
            projectStorage.Dispose();
            playerNames.Dispose();
        }
    }
}
