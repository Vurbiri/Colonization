using UnityEngine;

namespace VurbiriEditor.Colonization
{
    public class ReorderableListTest : MonoBehaviour
	{
		public Data[] values;

		[System.Serializable]
        public struct Data
		{
			public int index;
			public string name;
		}
    }
}
