//Assets\Vurbiri.International\Runtime\Utility\FileIdAndKey.cs
using System;

namespace Vurbiri.International
{
    [Serializable]
    public struct FileIdAndKey
	{
		public int id;
		public string key;

		public FileIdAndKey(int id, string key)
		{
			this.id = id;
			this.key = key;
		}
    }
}
