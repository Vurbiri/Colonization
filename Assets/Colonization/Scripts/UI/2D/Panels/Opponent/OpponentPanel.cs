using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	sealed public class OpponentPanel : AHintElement 
	{
		private const string SEPARATOR = "|";

		[SerializeField, Id(typeof(PlayerId))] private int _id;
		[Space]
		[SerializeField] private Image _icon;
		[SerializeField] private Image _diplomacy;
		[Space]
		[SerializeField] private Image _indicator;
		[SerializeField] private Image _indicatorTurn;
		[SerializeField, Range(0.1f, 2f)] private float _indicatorSpeed;
		[SerializeField] private WaitRealtime _waitStartIndicator;
		[Space]
		[SerializeField] private Sprite _friend;
		[SerializeField] private Sprite _enemy;
		[Space]
		[SerializeField] private PopupTextWidgetUI _popup;

		private Subscription _unsub;
		private int _relation, _min, _max;
		private bool _indicatorRun;

		public void Init(Vector3 offsetPopup)
		{
			var diplomacy = GameContainer.Diplomacy;
			var names = GameContainer.UI.PlayerNames;

			_popup.Init(offsetPopup);
			base.InternalInit(HintId.Canvas);

			_min = diplomacy.Min; _max = diplomacy.Max;

			_unsub = diplomacy.Subscribe(OnDiplomacy, false);
			SetRelation(diplomacy.GetRelationToPerson(_id), names[_id]);

			_icon.color = GameContainer.UI.PlayerColors[_id]; _icon = null;
			_unsub += names.Subscribe(_id, name => SetRelation(_relation, name), false);

			_indicator.canvasRenderer.SetAlpha(0f); _indicatorTurn.canvasRenderer.SetAlpha(0f);
			GameContainer.GameEvents.Subscribe(IndicatorTurn);
		}

		private void OnDiplomacy(Diplomacy diplomacy)
		{
			int relation = diplomacy.GetRelationToPerson(_id);

			if (_relation != relation)
			{
				_popup.ForceRun(relation - _relation);
				SetRelation(relation, GameContainer.UI.PlayerNames[_id]);
			}
		}

		private void SetRelation(int relation, string name)
		{
			_relation = relation;

			StringBuilder sb = new(64);
			sb.AppendLine(name);
			sb.AppendLine(CONST_UI.SEPARATOR);

			if (relation > 0)
			{
				sb.Append(GameContainer.UI.Colors.TextPositiveTag);
				sb.Append(relation.ToStr()); sb.Append(SEPARATOR); sb.Append(_max.ToStr());
				_diplomacy.sprite = _friend;
			}
			else
			{
				sb.Append(GameContainer.UI.Colors.TextNegativeTag);
				sb.Append((relation - 1).ToStr()); sb.Append(SEPARATOR); sb.Append(_min.ToStr());
				_diplomacy.sprite = _enemy;
			}
			
			_hintText = sb.ToString();
		}

		private void IndicatorTurn(Id<GameModeId> gameMode, TurnQueue turn)
		{
			bool run = turn.currentId == _id && (gameMode >= GameModeId.StartTurn & gameMode <= GameModeId.Play);
			if (run & (run ^ _indicatorRun))
				StartCoroutine(IndicatorTurn_Cn());
			_indicatorRun = run;
		}
		private IEnumerator IndicatorTurn_Cn()
		{
			float start = 0f, end = 1f, progress, sign;
			_indicator.fillClockwise = true;

			yield return _waitStartIndicator.Restart();

			_indicator.canvasRenderer.SetAlpha(1f);
			_indicatorTurn.canvasRenderer.SetAlpha(1f);

			while (_indicatorRun | !_indicator.fillClockwise)
			{
				progress = 0f; sign = end - start;

				do
				{
					progress += Time.unscaledDeltaTime * _indicatorSpeed;
					_indicator.fillAmount = start + sign * progress;
					yield return null;
				}
				while (progress < 1f);

				(start, end) = (end, start);
				_indicator.fillClockwise = !_indicator.fillClockwise;
			}

			_indicator.canvasRenderer.SetAlpha(0f);
			_indicatorTurn.canvasRenderer.SetAlpha(0f);
		}

		private void OnDestroy()
		{
			_unsub?.Dispose();
		}

#if UNITY_EDITOR

		public Vector2 UpdateVisuals_Editor(float offset)
		{
			var rectTransform = (RectTransform)transform.parent;
			rectTransform.anchoredPosition = new(offset, 0f);

			return rectTransform.sizeDelta;
		}

		private void OnValidate()
		{
			if (Application.isPlaying) return;

			this.SetChildren(ref _icon, "Icon");
			this.SetChildren(ref _diplomacy, "Relation");
			this.SetChildren(ref _indicator, "Indicator");
			this.SetChildren(ref _indicatorTurn, "IndicatorTurn");

			EUtility.SetAsset(ref _friend, "SP_IconFriend");
			EUtility.SetAsset(ref _enemy, "SP_IconEnemy");
		}
#endif
	}
}
