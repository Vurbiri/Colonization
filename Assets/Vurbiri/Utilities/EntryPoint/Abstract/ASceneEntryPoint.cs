namespace Vurbiri
{
    public abstract class ASceneEntryPoint : ASingleton<ASceneEntryPoint>
    {
        public abstract void Enter(SceneContainers containers);

        protected override void Awake()
        {
            _isNotDestroying = false;
            base.Awake();
        }
    }
}
