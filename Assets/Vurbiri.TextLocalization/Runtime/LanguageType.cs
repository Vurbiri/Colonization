//Assets\Vurbiri.TextLocalization\Runtime\LanguageType.cs
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace Vurbiri.TextLocalization
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

        public int Id => _id;
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

        public LanguageType(int id, LanguageType other)
        {
            _id = id;
            _code = other._code;
            _name = other._name;
            _folder = other._folder;
            _spriteName = other._spriteName;
        }

        public void LoadSprite()
        {
            string path = Path.Combine(_folder, _spriteName);

            try 
            {
                _sprite = Resources.Load<Sprite>(path);
            }
            catch (Exception ex)
            {
                Message.Error($"Localization. Error loading sprite {path} for {_name}\n".Concat(ex.Message));
            }
        }

        public int CompareTo(LanguageType other) => _id.CompareTo(other._id);
    }
}
