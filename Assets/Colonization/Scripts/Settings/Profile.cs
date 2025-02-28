//Assets\Colonization\Scripts\Settings\Profile.cs
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public partial class Settings
    {
        [System.Serializable]
        private class Profile
        {
            public int idLang = 1;
            public int quality = 2;

            private const int SIZE_ARRAY_ADD = 2;

            public void FromArray(IReadOnlyList<int> loadData)
            {
                if (loadData == null || loadData.Count != SIZE_ARRAY_ADD)
                    return;

                int i = 0;
                idLang = loadData[i++];
                quality = loadData[i];
            }

            public int[] ToArray()
            {
                int[] array = new int[SIZE_ARRAY_ADD];

                int i = 0;
                array[i++] = idLang;
                array[i++] = quality;

                return array;
            }
        }
    }
}
