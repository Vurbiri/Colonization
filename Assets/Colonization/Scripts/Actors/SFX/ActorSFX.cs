using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(AudioSource)), DisallowMultipleComponent]
    public abstract class ActorSFX : MonoBehaviour
    {
        [SerializeField] private HitSFXName _mainProfitSFX;
        [SerializeField] private HitSFXName _advProfitSFX;

        private readonly WaitSignal _waitProfit = new();

        protected ReadOnlyArray<string> _hitSFX;
        protected Transform _thisTransform;
        protected AudioSource _audioSource;

        public virtual Transform TargetTransform => _thisTransform;
        public virtual Vector3 Position { [Impl(256)] get => _thisTransform.position; }

        [Impl(256)] protected void InitInternal(ReadOnlyArray<string> hitSFX)
		{
            _thisTransform = GetComponent<Transform>();
            _audioSource = GetComponent<AudioSource>();
            _hitSFX = hitSFX;
        }

        [Impl(256)] public void Play(AudioClip clip) => _audioSource.PlayOneShot(clip);

        [Impl(256)] public Coroutine Hit(int idSkill, ActorSkin target) => StartCoroutine(GameContainer.SFX.Run(_hitSFX[idSkill], this, target));

        [Impl(256)] public WaitSignal MainProfit(bool isPerson, ActorSkin target)
        {
            StartCoroutine(Profit_Cn(isPerson, target, _mainProfitSFX));
            return _waitProfit.Restart();
        }
        [Impl(256)] public WaitSignal AdvProfit(bool isPerson, ActorSkin target)
        {
            StartCoroutine(Profit_Cn(isPerson, target, _advProfitSFX));
            return _waitProfit.Restart();
        }

        public virtual void Death() { }
        public Coroutine PostDeath(float height)
        {
            return StartCoroutine(Death_Cn(height));

            //Local
            IEnumerator Death_Cn(float height)
            {
                Vector3 position = _thisTransform.localPosition;
                while (position.y > height)
                {
                    position.y -= 3f * Time.unscaledDeltaTime;
                    _thisTransform.localPosition = position;
                    yield return null;
                }
            }
        }

        private IEnumerator Profit_Cn(bool isPerson, ActorSkin target, string name)
        {
            if (isPerson | GameContainer.GameSettings.TrackingCamera)
                yield return GameContainer.CameraController.ToPosition(_thisTransform.position, true);

            yield return GameContainer.SFX.Run(name, this, target);

            _waitProfit.Send();
        }

#if UNITY_EDITOR
        protected virtual void SetProfitSFX_Ed(string mainProfitSFX, string advProfitSFX)
        {
            if(string.IsNullOrEmpty(_mainProfitSFX?.Value))
                _mainProfitSFX = new(mainProfitSFX);
            if (string.IsNullOrEmpty(_advProfitSFX?.Value))
                _advProfitSFX = new(advProfitSFX);
        }
#endif
    }
}
