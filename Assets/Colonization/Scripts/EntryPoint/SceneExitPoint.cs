using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class SceneExitPoint : ASceneExitPoint
    {
        public SceneExitPoint(int nextScene, IContainer sceneContainers) : base(new(nextScene), sceneContainers)
        {
        }
    }
}
