using System;
using System.Collections.Generic;
using UnityEngine;

namespace VurbiriEditor.International
{
	[Serializable]
	internal class LanguageRecord
	{
		[SerializeField] private string _key;
		[SerializeField] private List<ItemRecord> _values;

		public string Key { get => _key;}

		public LanguageRecord(string key)
		{ 
			_key = key;
			_values = new();
		}

		public void Add(string name, string text) => _values.Add(new(name, text));

		public string GetText(int id) => _values[id].text;

		#region Nested: ItemRecord
		//***********************************
		[Serializable]
		private class ItemRecord
		{
			public string name;
			public string text;

			public ItemRecord(string name, string text)
			{
				this.name = name;
				this.text = text;
			}
		}
		#endregion
	}
}
