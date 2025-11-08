namespace Vurbiri.Colonization
{
    sealed public class Shrine : AEdifice
    {
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (UnityEngine.Application.isPlaying) return;

            _settings.id = EdificeId.Shrine;
            _settings.groupId = EdificeGroupId.Shrine;
            _settings.nextId = EdificeId.None;
            _settings.nextGroupId = EdificeGroupId.None;

            _settings.isBuildWall = false;
            _settings.isUpgrade = false;

            _settings.profit = 0;
            _settings.wallDefense = 0;

            base.OnValidate();
        }
#endif
    }
}
