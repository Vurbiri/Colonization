//Assets\Colonization\Scripts\Settings\Profile.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Audio;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public partial class Settings
    {
        [System.Serializable]
        private class Profile
        {
            public int idLang = 1;
            [Space]
            public int quality = 2;
            [Space]
            public bool mute = false;
            public float volumeGeneric = 1f;
            public IdArray<AudioTypeId, float> volumes = new(0.6f);

            private const float TO_RATE_VOLUME = 100f, FROM_RATE_VOLUME = 1f / TO_RATE_VOLUME;
            private const int SIZE_ARRAY_ADD = 4;

            public void FromArray(IReadOnlyList<int> loadData)
            {
                if (loadData == null || loadData.Count != AudioTypeId.Count + SIZE_ARRAY_ADD)
                    return;

                volumes = new(); int i = 0;
                for (; i < AudioTypeId.Count; i++)
                    volumes[i] = FROM_RATE_VOLUME * loadData[i];
                volumeGeneric = FROM_RATE_VOLUME * loadData[i++];

                mute = loadData[i++] > 0;

                idLang = loadData[i++];
                quality = loadData[i];
            }

            public int[] ToArray()
            {
                int[] array = new int[AudioTypeId.Count + SIZE_ARRAY_ADD];

                int i = 0;
                for (; i < AudioTypeId.Count; i++)
                    array[i] = Mathf.RoundToInt(volumes[i] * TO_RATE_VOLUME);
                array[i++] = Mathf.RoundToInt(volumeGeneric * TO_RATE_VOLUME);

                array[i++] = mute ? 1 : 0;

                array[i++] = idLang;
                array[i++] = quality;

                return array;
            }
        }
    }
}
