using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable]
	sealed public class MoveUsingLerp : AMoveUsingLerp
	{
		protected override float DeltaTime { [Impl(256)] get => UnityEngine.Time.unscaledDeltaTime; }

		[Impl(256)] public MoveUsingLerp(Transform transform, float speed) : base(transform, speed) { }
	}
}
