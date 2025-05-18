//Assets\Vurbiri.International\Runtime\LanguageType.cs
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Vurbiri.International
{
    [Serializable]
    public class LanguageType : IEquatable<SystemLanguage>
    {
        [SerializeField] private SystemLanguage _id;
        [SerializeField] private string _code;
        [SerializeField] private string _name;
        [SerializeField] private string _folder;
        [SerializeField] private string _spriteName = "Banner";

        [JsonIgnore] private Sprite _sprite;

        public SystemLanguage Id => _id;
        public string Code => _code;
        public string Name => _name;
        public string Folder => _folder;
        public string SpriteName => _spriteName;

        [JsonConstructor]
        public LanguageType(SystemLanguage id, string code, string name, string folder, string spriteName)
        {
            _id = id;
            _code = code;
            _name = name;
            _folder = folder;
            _spriteName = spriteName;
        }

        public Sprite GetSprite()
        {
            if (_sprite == null)
                _sprite = Resources.Load<Sprite>(string.Concat(_folder, "/", _spriteName));
            return _sprite;
        }

        public bool Equals(SystemLanguage id) => _id == id;
    }
}
