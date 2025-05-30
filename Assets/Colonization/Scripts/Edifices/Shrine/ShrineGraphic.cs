using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Vurbiri.Colonization
{
    sealed public class ShrineGraphic : AEdificeGraphic
    {
        [Space]
        [SerializeField] private ParticleSystem _pillarFlame;
        [Header("Color")]
        [SerializeField, Range(0f, 1f)] private float _alfa = 0.85f;
        [SerializeField, Range(0f, 1f)] private float _brightness = 0.75f;

        public override WaitSignal Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links, bool isSFX)
        {
            HumanMaterials visual = SceneContainer.Get<HumansMaterials>()[playerId];

            GetComponent<MeshRenderer>().SetSharedMaterial(visual.materialUnlit, _idMaterial);

            MainModule main = _pillarFlame.main;
            Color color = visual.color.SetAlpha(_alfa);
            main.startColor = new(color.Brightness(_brightness), color.Brightness(2f - _brightness)) { mode = ParticleSystemGradientMode.TwoColors };

            var signal = isSFX ? _edificeSFX.Run(transform) : _edificeSFX.Destroy();
            StartCoroutine(SFX_Cn(signal));
            return signal;
        }

        private IEnumerator SFX_Cn(WaitSignal signal)
        {
            yield return signal;
            _pillarFlame.Play();
            Destroy(this);
        }
    }
}
