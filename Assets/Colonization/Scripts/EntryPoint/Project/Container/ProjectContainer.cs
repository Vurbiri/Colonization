using System.Runtime.CompilerServices;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
	public class ProjectContainer : IContainer
	{
        private static ProjectContent s_projectContent;

        public static Settings Settings
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_projectContent.settings;
        }
        public static GameSettings GameSettings
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_projectContent.gameSettings;
        }

        public static Prices Prices
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_projectContent.prices;
        }

        public static PlayerColors PlayerColors
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_projectContent.playerColors;
        }
        public static PlayerNames PlayerNames
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_projectContent.playerNames;
        }
        public static HumansMaterials Materials
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_projectContent.humansMaterials;
        }

        public static IStorageService StorageService
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_projectContent.storageService;
        }

        public static YandexSDK YSDK
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_projectContent.ysdk;
        }

        public class UI
        {
            public static ProjectColors Colors
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => s_projectContent.projectColors;
            }
        }

        public ProjectContainer() { }
        public ProjectContainer(ProjectContent content) => s_projectContent ??= content;

        public virtual void Dispose() => s_projectContent.Dispose();
    }
}
