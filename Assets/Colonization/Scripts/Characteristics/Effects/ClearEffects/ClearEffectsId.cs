using System.Runtime.CompilerServices;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.Characteristics
{
	public abstract class ClearEffectsId : IdType<ClearEffectsId>
	{
        public const int Positive = 0;
		public const int Negative = 1;
        public const int All	  = 2;

		[NotId] public const int Code = 99; 

        static ClearEffectsId() => ConstructorRun();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ColorSelection(ProjectColors colors, int id) => id switch
		{
			Positive => colors.TextNegativeTag,
			Negative => colors.TextPositiveTag,
			All		 => colors.TextWarningTag,
			_ => Errors.ArgumentOutOfRange("ClearEffectsId", id.ToString()),
		};
	}
}
