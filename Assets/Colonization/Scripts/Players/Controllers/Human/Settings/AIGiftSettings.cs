using Vurbiri.International;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class AIGiftSettings
	{
        public FileIdAndKey msg;
        public IntRnd ratio; // = new(2, 8);
        public int shiftRelation; // = 3;
        public int shiftMax; // = 2;
        public int shiftAmount; // = 3;
        public int minAmount; // = 7;
    }
}
