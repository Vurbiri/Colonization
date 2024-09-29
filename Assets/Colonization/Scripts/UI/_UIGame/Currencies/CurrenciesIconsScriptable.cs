using NaughtyAttributes;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [CreateAssetMenu(fileName = "CurrenciesIcons", menuName = "Vurbiri/Colonization/CurrenciesIcons", order = 51)]
    public class CurrenciesIconsScriptable : ScriptableObject
    {
        [SerializeField] private EnumArray<CurrencyType, CurrencyIcon> _icons;
        [Space]
        [SerializeField] private CurrencyIcon _blood;
        [Space]
        [SerializeField] private string _path = "Assets/Colonization/Import/UI/IconCurrencies/IconCurrency_Atlas.asset";

        public CurrencyIcon this[int index] => _icons[index];
        public CurrencyIcon this[CurrencyType type] => _icons[(int) type];

        public CurrencyIcon Blood => _blood;

        [Button]
        public void CreateAtlas()
        {
            Texture2D texture = _blood.Icon.texture;
            int width = texture.width, height = texture.height;
            Texture2D textureNew = new(width, height);
            Color color = _blood.Color;

            Debug.Log(Application.dataPath);

            //    for (int i = 0; i < width; i++)
            //    {
            //        for (int j = 0; j < height; j++)
            //        {
            //            //color = _blood.Color * texture.GetPixel(i, j).a;
            //            textureNew.SetPixel(i, j, color);
            //            Debug.Log(textureNew.GetPixel(i, j));
            //        }
            //    }

            //    textureNew.Apply();

            //    AssetDatabase.CreateAsset(Sprite.Create(textureNew, new(0, 0, width, height), new(width / 2, height / 2)), _path);
            //    AssetDatabase.SaveAssets();
        }

    }
}
