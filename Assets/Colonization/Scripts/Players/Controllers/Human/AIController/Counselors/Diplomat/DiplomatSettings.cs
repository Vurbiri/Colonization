using UnityEngine;
using Vurbiri.International;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class DiplomatSettings
	{
        [Header("Occupation")]
        public FileIdAndKey colonyMsg;
        public IntRnd colonyPenalty; // = new(-3, -1);
        [Range(0, 100)] public int colonyRelationOffset;
        public FileIdAndKey portMsg;
        public IntRnd portPenalty;// = new(-5, -2);
        [Range(0, 100)] public int portRelationOffset;
        [Header("Gift")]
        public FileIdAndKey giftMsg;
        public IntRnd ratio; // = new(2, 8);
        [Range(0, 100)] public int relationOffset; // = 22;
        public int shiftMax; // = 2;
        public int shiftAmount; // = 3;
        public int minAmount; // = 7;
    }
}
