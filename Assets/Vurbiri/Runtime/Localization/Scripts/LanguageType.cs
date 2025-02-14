//Assets\Vurbiri\Runtime\Localization\Scripts\LanguageType.cs
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace Vurbiri.Localization
{
    [Serializable]
    public class LanguageType : IComparable<LanguageType>
    {
        [SerializeField] private int _id;
        [SerializeField] private string _code;
        [SerializeField] private string _name;
        [SerializeField] private string _folder;
        [SerializeField] private string _spriteName = "Banner";
        [SerializeField] private Sprite _sprite;

        public int Id { get => _id; set => _id = value; }
        public string Code => _code;
        public string Name => _name;
        public string Folder => _folder;
        public string SpriteName => _spriteName;
        [JsonIgnore]
        public Sprite Sprite => _sprite;

        [JsonConstructor]
        public LanguageType(int id, string code, string name, string folder, string spriteName)
        {
            _id = id;
            _code = code;
            _name = name;
            _folder = folder;
            _spriteName = spriteName;
        }

        public bool LoadSprite()
        {
            string path = Path.Combine(_folder, _spriteName);

            try 
            {
                _sprite = Resources.Load<Sprite>(path);
                return true;
            }
            catch (Exception ex)
            {
                Message.Error($"--- Ошибка загрузки спрайта {_spriteName} по пути {path}  ---\n".Concat(ex.Message));
                return false;
            }
        }

        public int CompareTo(LanguageType other) => _id.CompareTo(other._id);
    }
}
