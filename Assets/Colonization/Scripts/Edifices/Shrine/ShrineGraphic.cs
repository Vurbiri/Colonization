//Assets\Colonization\Scripts\Edifices\Shrine\ShrineGraphic.cs
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Vurbiri.Colonization
{
    sealed public class ShrineGraphic : AEdificeGraphicReColor
    {
        [Space]
        [SerializeField] private ParticleSystem _pillarFlame;
        [Header("Color")]
        [SerializeField, Range(0f, 1f)] private float _alfa = 0.85f;
        [SerializeField, Range(0f, 1f)] private float _brightness = 0.75f;

        public override void Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            PlayerVisual visual = SceneData.Get<PlayersVisual>()[playerId];

            GetComponent<MeshRenderer>().SetSharedMaterial(visual.materialUnlit, _idMaterial);

            MainModule main = _pillarFlame.main;
            Color color = visual.color.SetAlpha(_alfa);
            main.startColor = new(color.Brightness(_brightness), color.Brightness(2f - _brightness)) { mode = ParticleSystemGradientMode.TwoColors };

            _pillarFlame.Play();
        }

    }
}
