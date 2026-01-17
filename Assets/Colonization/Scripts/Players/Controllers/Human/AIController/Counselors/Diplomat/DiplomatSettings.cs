using UnityEngine;
using Vurbiri.International;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class DiplomatSettings
	{
        [Header("Occupation")]
        public FileIdAndKey colonyMsg;
        public RndInt colonyPenalty; // = new(-3, -1);
        [Range(0, 100)] public int colonyRelationOffset;
        public FileIdAndKey portMsg;
        public RndInt portPenalty;// = new(-5, -2);
        [Range(0, 100)] public int portRelationOffset;
        [Header("Gift")]
        public FileIdAndKey giftMsg;
        public RndInt ratio; // = new(2, 8);
        [Range(0, 100)] public int relationOffset; // = 22;
        public int shiftMax; // = 2;
        public int shiftAmount; // = 3;
        public int minAmount; // = 7;
        [Header("Other")]
        [Range(0f, 10f)] public float bannerTime;
    }
}
