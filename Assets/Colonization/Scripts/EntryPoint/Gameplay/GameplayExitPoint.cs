//Assets\Colonization\Scripts\EntryPoint\Gameplay\GameplayExitPoint.cs
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class GameplayExitPoint : ASceneExitPoint
    {
        public GameplayExitPoint(int nextScene, SceneContainer sceneContainers) : base(new(nextScene), sceneContainers)
        {
        }
    }
}
