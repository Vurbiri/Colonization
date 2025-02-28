//Assets\Vurbiri.Audio\AudioMixer\IAudioMixer.cs
namespace Vurbiri
{
    public interface IVolume<T> where T : IdType<T>
    {
        public float this[int index] { get; set; }
        public float this[Id<T> index] { get; set; }
    }
}
