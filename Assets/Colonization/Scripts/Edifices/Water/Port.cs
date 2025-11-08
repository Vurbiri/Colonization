namespace Vurbiri.Colonization
{
    sealed public class Port : AEdifice
    {
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (UnityEngine.Application.isPlaying) return;

            _settings.id = System.Math.Clamp(_settings.id, EdificeId.PortOne, EdificeId.LighthouseTwo);
            _settings.groupId = EdificeGroupId.Port;

            if (_settings.id < EdificeId.LighthouseOne)
            {
                _settings.nextId = _settings.id + 2;
                _settings.nextGroupId = EdificeGroupId.Port;
                _settings.profit = 1;
            }
            else
            {
                _settings.nextId = EdificeId.None;
                _settings.nextGroupId = EdificeGroupId.None;
                _settings.profit = 2;
            }

            _settings.isUpgrade = _settings.nextId != EdificeId.None;

            _settings.isBuildWall = false;
            _settings.wallDefense = 0;

            base.OnValidate();
        }
#endif
    }
}
