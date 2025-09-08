namespace Vurbiri
{
    public abstract class Enumerator : System.Collections.IEnumerator
    {
        public object Current => null;

        public abstract bool MoveNext();

        public void Reset() { }
    }
}
