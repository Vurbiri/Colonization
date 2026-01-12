namespace Vurbiri
{
	public abstract class AWait : System.Collections.IEnumerator
	{
		public object Current => null;

		public abstract bool IsWait { get; }

		public abstract bool MoveNext();

		public virtual void Reset() { }
	}
}
