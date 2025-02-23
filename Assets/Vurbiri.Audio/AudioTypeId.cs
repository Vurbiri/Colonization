//Assets\Vurbiri.Audio\AudioTypeId.cs
namespace Vurbiri.Audio
{
    public abstract class AudioTypeId : IdType<AudioTypeId>
    {
        public const int Music   = 0;
        public const int SFX     = 1;
        public const int Ambient = 2;

        static AudioTypeId() => RunConstructor();
    }
}
