using Vurbiri.Colonization.Storage;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    public class ProjectContent : IContainerContent
    {
        public Settings settings;
        public GameSettings gameSettings;

        public ProjectColors projectColors;
        public PlayerColors playerColors;
        public PlayerNames playerNames;
        public HumansMaterials humansMaterials;

        public IStorageService storageService;
        public ProjectStorage projectStorage;
        public YandexSDK ysdk;

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
