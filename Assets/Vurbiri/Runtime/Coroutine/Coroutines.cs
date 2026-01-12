using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	sealed public class Coroutines : MonoBehaviour
	{
		internal static Coroutines s_instance;

		private void Awake()
		{
			if (s_instance == null)
			{
				DontDestroyOnLoad(gameObject);
				s_instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public static void Create()
		{
			if (s_instance == null && Application.isPlaying)
				new GameObject("[Coroutine]").AddComponent<Coroutines>();
		}

		private void OnDestroy()
		{
			if(s_instance == this)
				s_instance = null;
		}
	}

	public static class CoroutineExt
	{
		static CoroutineExt() => Coroutines.Create();

		[Impl(256)] public static Coroutine Start(this IEnumerator routine) => Coroutines.s_instance.StartCoroutine(routine);

		[Impl(256)] public static void Stop(this Coroutine routine) => Coroutines.s_instance.StopCoroutine(routine);
	}
}
