using Vurbiri.International;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class DiplomatSettings
	{
        public FileIdAndKey msg;
        public IntRnd ratio; // = new(2, 8);
        public int relationOffset; // = 22;
        public int shiftMax; // = 2;
        public int shiftAmount; // = 3;
        public int minAmount; // = 7;
    }
}
