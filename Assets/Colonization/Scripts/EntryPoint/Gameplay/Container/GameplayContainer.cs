using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
	public class GameplayContainer : ProjectContainer
    {
        private static GameplayContent s_content;

        public static GameplayStorage Storage
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.storage;
        }

        public static GameLoop Game
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.gameLoop;
        }

        public static InputController InputController
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.inputController;
        }

        public GameplayContainer(GameplayContent content)
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
