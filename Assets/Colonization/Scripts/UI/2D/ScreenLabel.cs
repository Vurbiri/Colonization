using System.Collections;
using TMPro;
using UnityEngine;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
	public class ScreenLabel : MonoBehaviour
	{
        [SerializeField, Key(Files.Gameplay)] private string _landingKey;
        [SerializeField, Key(Files.Gameplay)] private string _startTurnKey;
        [Space]
        [SerializeField, Range(5f, 15f)] private float _onSpeed = 9f;
        [SerializeField, Range(0.1f, 1.5f)] private float _offSpeed = 0.9f;
        [SerializeField, MinMax(1f, 5f)] private WaitRealtime _showTime = 2f;

        private TextMeshProUGUI _label;
        private CanvasRenderer _renderer;
        private WaitRealtime _delayStartTurn;

        private string _landingText, _startTurnText;

        private PlayerNames _names;
        private readonly string[] _hexColors = new string[PlayerId.Count];

        public void Init()
		{
            _label = GetComponent<TextMeshProUGUI>();
            _renderer = _label.canvasRenderer;
            _renderer.SetAlpha(0f);

            _delayStartTurn = new((1f/_onSpeed + _showTime.Time + 1f/_offSpeed) * 0.51f);

            _names = SceneContainer.Get<PlayerNames>();

            var colors = SceneContainer.Get<PlayerColors>();
            for (int i = 0; i < PlayerId.Count; i++)
                _hexColors[i] = colors[i].ToHex();

            Localization.Instance.Subscribe(SetText);

        }

        public void Landing(int id)
        {
            _label.text = string.Format(_landingText, _hexColors[id], _names[id] );

            StartCoroutine(Label_Cn());
        }

        public IEnumerator StartTurn_Wait(int turn, int id)
        {
            _label.text = string.Format(_startTurnText, turn, _hexColors[id], _names[id]);

            StartCoroutine(Label_Cn());

            yield return _delayStartTurn.Restart();
        }

        private IEnumerator Label_Cn()
        {
            float alpha = 0f;

            while (alpha < 1f)
            {
                alpha += Time.unscaledDeltaTime * _onSpeed;
                _renderer.SetAlpha(alpha);
                yield return null;
            }

            _renderer.SetAlpha(alpha = 1f);

            yield return _showTime.Restart();

            while (alpha > 0f)
            {
                alpha -= Time.unscaledDeltaTime * _offSpeed;
                _renderer.SetAlpha(alpha);
                yield return null;
            }

            _renderer.SetAlpha(0f);
        }

        private void SetText(Localization localization)
		{
            _landingText = localization.GetText(Files.Gameplay, _landingKey);
            _startTurnText = localization.GetText(Files.Gameplay, _startTurnKey);
        }
	}
}
