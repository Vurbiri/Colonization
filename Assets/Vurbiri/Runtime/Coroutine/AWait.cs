namespace Vurbiri
{
    public abstract class AWait : System.Collections.IEnumerator
    {
        public object Current => null;

        public abstract bool MoveNext();

        public void Reset() { }
    }
}
