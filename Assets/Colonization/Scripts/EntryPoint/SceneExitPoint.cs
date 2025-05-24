using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class SceneExitPoint : ASceneExitPoint
    {
        public SceneExitPoint(int nextScene, SceneContainer sceneContainers) : base(new(nextScene), sceneContainers)
        {
        }
    }
}
