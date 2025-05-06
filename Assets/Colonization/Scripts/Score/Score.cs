//Assets\Colonization\Scripts\Score\Score.cs
namespace Vurbiri.Colonization
{
    public class Score
	{
		private readonly int[] _values;

		public Score()
		{
			_values = new int[PlayerId.HumansCount];
		}

		public Score(int[] values)
		{
			_values = values;
		}
	}
}
