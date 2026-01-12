using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable]
	sealed public class ScaledMoveUsingLerp : AMoveUsingLerp
	{
		protected override float DeltaTime { [Impl(256)] get => UnityEngine.Time.deltaTime; }

		[Impl(256)] public ScaledMoveUsingLerp(Transform transform, float speed) : base(transform, speed) { }
	}
}
