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

        public override void Initialize(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            Player player = Players.Instance[owner];

            GetComponent<MeshRenderer>().SetSharedMaterial(player.Material, _idMaterial);

            MainModule main = _pillarFlame.main;
            Color color = player.Color.SetAlpha(_alfa);
            main.startColor = new(color.Brightness(_brightness), color.Brightness(2f - _brightness)) { mode = ParticleSystemGradientMode.TwoColors};

            _pillarFlame.Play();
        }

    }
}
