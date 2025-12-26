using System;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Storage;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	[Serializable]
	public partial class PlayerColors
	{
		[SerializeField] private Array<Color32> _colors;

		private readonly Array<Color32> _customs = new(PlayerId.HumansCount);
		private readonly Color32[] _defaults = new Color32[PlayerId.HumansCount];

		private readonly VAction<ReadOnlyArray<Color32>> _customsChange = new();
		private readonly VAction<ReadOnlyArray<Color32>> _colorsChange = new();

		public Color32 this[int index] { [Impl(256)] get => _colors[index]; }
		public Color32 this[Id<PlayerId> id] { [Impl(256)] get => _colors[id.Value]; }

		public PlayerColors Init(ProjectStorage storage)
		{
			if (storage.TryLoadPlayerColors(out Color32[] customs))
				_customs.SetArray(customs);

			Color32 custom;
			for (int index = 0; index < PlayerId.HumansCount; ++index)
			{
				_defaults[index] = _colors[index];

				custom = _customs[index];
				if(custom.a > 0)  _colors[index] = custom;
			}

			storage.BindPlayerColors(_customsChange);

			return this;
		}

		[Impl(256)] public Subscription Subscribe(Action<ReadOnlyArray<Color32>> action, bool instantGetValue = true) => _colorsChange.Add(action, _colors, instantGetValue);
		[Impl(256)] public void Unsubscribe(Action<ReadOnlyArray<Color32>> action) => _colorsChange.Remove(action);

		[Impl(256)] public Color GetDefault(int id) => _defaults[id];

		public void TrySetCustomColors(Color32[] colors)
		{
			bool change = false; Color32 color;
			for (int i = 0; i < PlayerId.HumansCount; ++i)
			{
				color = colors[i];
				if (_defaults[i].IsEquals(color))
				{
					if (_customs[i].a > 0)
					{
						_customs[i] = new();
						_colors[i] = _defaults[i];

						change = true;
					}
				}
				else
				{
					if (!_customs[i].IsEquals(color))
					{
						_customs[i] = color;
						_colors[i] = color;

						change = true;
					}
				}
			}

			if (change)
			{
				_customsChange.Invoke(_customs);
				_colorsChange.Invoke(_colors);
			}
		}

		[Impl(256)] public static implicit operator ReadOnlyArray<Color32>(PlayerColors playerColors) => playerColors._colors;

#if UNITY_EDITOR
		public void OnValidate()
		{
			_colors ??= new Color32[] { new(97, 54, 196, 255), new(13, 145, 58, 255), new(181, 227, 144, 255), new(145, 0, 0, 255) };
		}
#endif
	}
}
