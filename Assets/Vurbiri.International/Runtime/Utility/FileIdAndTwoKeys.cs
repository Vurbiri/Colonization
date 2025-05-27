using System;

namespace Vurbiri.International
{
    [Serializable]
    public struct FileIdAndTwoKeys
	{
        public int id;
        public string keyA;
        public string keyB;

        public FileIdAndTwoKeys(int id, string keyA, string keyB)
        {
            this.id = id;
            this.keyA = keyA;
            this.keyB = keyB;
        }
    }
}
