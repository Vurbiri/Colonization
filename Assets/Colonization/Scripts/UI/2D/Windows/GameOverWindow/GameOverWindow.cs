using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.International;
using Vurbiri.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class GameOverWindow : MonoBehaviour
	{
        [SerializeField] private TextMeshProUGUI _caption;
        [SerializeField] private Caption _victory;
        [SerializeField] private Caption _defeat;
		[Space]
        [SerializeField] private TextMeshProUGUI _winner;
        [SerializeField] private FileIdAndKey _winnerKey;
        [Space]
        [SerializeField] private Place[] _places;

        private readonly VAction<int> _onOpen = new();
        private int _id;

        public void Init(int id, Action<int> onOpenWindow)
        {
            _id = id;
            _onOpen.Add(onOpenWindow);

            GameContainer.Chaos.OnGameOver.Add(GameOver);
            gameObject.SetActive(false);
        }

        private void GameOver(Winner winner)
        {
            _onOpen.Invoke(_id);
            GameContainer.CameraController.ToDefaultPosition();

            GetComponentInChildren<VButton>().AddListener(Transition.Exit);

            var scores = GetScores();
            for (int i = 0; i < PlayerId.HumansCount; ++i)
                _places[i].Set(scores[i]);

            int winnerId;
            if (winner == Winner.Satan)
            {
                _defeat.Set(_caption);
                winnerId = PlayerId.Satan;
            }
            else
            {
                _victory.Set(_caption);
                winnerId = scores[0].id;
            }

            _winner.text = Localization.Instance.GetFormatText(_winnerKey.id, _winnerKey.key, GameContainer.UI.PlayerColorNames[winnerId]);

            gameObject.SetActive(true);
        }

        [Impl(256)]
        private static List<Score> GetScores()
        {
            List<Score> scores = new (PlayerId.HumansCount);
            var score = GameContainer.Score;
            for (int i = 0; i < PlayerId.HumansCount; ++i)
                scores.Add(new(i, score[i]));
            scores.Sort();
            return scores;
        }

        #region Nested Caption, Score
        // ************************************************
        private readonly struct Score : IComparable<Score>
        {
            public readonly int id;
            public readonly int score;

            [Impl(256)]
            public Score(int id, int score)
            {
                this.id = id;
                this.score = score;
            }

            public readonly int CompareTo(Score other) => other.score - score;
        }
        // ************************************************
        [System.Serializable]
		private class Caption
		{
			[SerializeField] private FileIdAndKey _getText;
            [SerializeField] private Color _color;
            [SerializeField] private AudioClip _sound;

            [Impl(256)]
            public void Set(TextMeshProUGUI caption)
            {
                caption.color = _color;
                caption.text = Localization.Instance.GetText(_getText);
                GameContainer.Shared.Audio.PlayOneShot(_sound);
            }
		}
        // ************************************************
        [System.Serializable]
        private class Place
        {
            public TextMeshProUGUI player;
            public TextMeshProUGUI point;

            public void Set(Score score)
            {
                player.text = GameContainer.UI.PlayerNames[score.id];
                point.text = score.score.ToString();
            }

#if UNITY_EDITOR
            public void OnValidate(GameOverWindow parent, string name, float fontSize)
            {
                parent.SetChildren(ref player, name);
                player.SetChildren(ref point, "Score");

                player.fontSize = point.fontSize = fontSize;
            }
#endif
        }
        // ************************************************
        #endregion

#if UNITY_EDITOR

        [StartEditor]
        [SerializeField, Range(20f, 40f)] private float _scoreFontSize = 30;
        [SerializeField, HideInInspector] private UnityEngine.UI.Image _mainImage;

        private void OnValidate()
        {
            this.SetChildren(ref _caption, "CaptionText");
            this.SetChildren(ref _winner, "WinnerText");

            EUtility.SetArray(ref _places, PlayerId.HumansCount);
            for (int i = 0; i < PlayerId.HumansCount; ++i)
            {
                _places[i] ??= new();
                _places[i].OnValidate(this, $"{i+1}stPlace", _scoreFontSize - i);
            }

            this.SetComponent(ref _mainImage);
        }

        public void UpdateVisuals_Ed(float pixelsPerUnit, ProjectColors colors)
        {
            Color color = colors.PanelBack.SetAlpha(1f);

            _mainImage.color = color;
            _mainImage.pixelsPerUnitMultiplier = pixelsPerUnit;
        }
#endif
    }
}
