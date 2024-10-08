using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Vurbiri.Colonization
{
    public class ShrineGraphic : AEdificeGraphic
    {
        [Space]
        [SerializeField] private ParticleSystem _pillarFlame;
        [Header("Color")]
        [SerializeField, Range(0f, 1f)] private float _alfa = 0.85f;
        [SerializeField, Range(0f, 1f)] private float _brightness = 0.75f;

        public override void Init(Id<PlayerId> playerId, IdHashSet<LinkId, CrossroadLink> links)
        {
            Player player = Players.Instance[playerId];

            GetComponent<MeshRenderer>().SetSharedMaterial(player.MaterialUnlit, _idMaterial);

            MainModule main = _pillarFlame.main;
            Color color = player.Color.SetAlpha(_alfa);
            main.startColor = new(color.Brightness(_brightness), color.Brightness(2f - _brightness)) { mode = ParticleSystemGradientMode.TwoColors};

            _pillarFlame.Play();
        }

    }
}
