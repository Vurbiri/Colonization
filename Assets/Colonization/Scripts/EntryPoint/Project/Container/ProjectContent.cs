using Vurbiri.Colonization.Storage;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.Storage;

namespace Vurbiri.Colonization
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

#if YSDK
        public Vurbiri.Yandex.YandexSDK ysdk;
		public Vurbiri.Yandex.YMoney money;
#endif

        public void Dispose() 
		{
			playerNames?.Dispose();
			projectStorage?.Dispose();
		}
	}
}
