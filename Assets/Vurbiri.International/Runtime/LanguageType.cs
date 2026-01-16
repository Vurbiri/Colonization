using Newtonsoft.Json;
using System;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.International
{
    [Serializable]
    sealed public class LanguageType : IEquatable<SystemLanguage>
    {
        [SerializeField] private SystemLanguage _id;
        [SerializeField] private string _code;
        [SerializeField] private string _name;
        [SerializeField] private string _folder;
        [SerializeField] private string _spriteName = "Banner";

        private Sprite _sprite;

        public SystemLanguage Id { [Impl(256)] get => _id; }
        public string Code { [Impl(256)] get => _code; }
        public string Name { [Impl(256)] get => _name; }
        public string Folder { [Impl(256)] get => _folder; }
        public string SpriteName { [Impl(256)] get => _spriteName; }

        [JsonConstructor]
        public LanguageType(SystemLanguage id, string code, string name, string folder, string spriteName)
        {
            _id = id;
            _code = code;
            _name = name;
            _folder = folder;
            _spriteName = spriteName;
        }

        [Impl(256)] public Sprite GetSprite()
        {
            if (_sprite == null)
                _sprite = Resources.Load<Sprite>(string.Concat(_folder, "/", _spriteName));
            return _sprite;
        }

        [Impl(256)] public bool CodeEquals(string code) => _code.ToLowerInvariant() == code.ToLowerInvariant();
        [Impl(256)] public bool Equals(SystemLanguage id) => _id == id;

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(obj, this)) return true;
            if (obj is LanguageType type) return type._id == _id;
            if (obj is SystemLanguage id) return id == _id;
            return false;
        }

        [Impl(256)] public override int GetHashCode() => _id.GetHashCode();

        [Impl(256)] public static bool operator ==(LanguageType type, SystemLanguage id) => type._id == id;
        [Impl(256)] public static bool operator !=(LanguageType type, SystemLanguage id) => type._id != id;
    }
}
