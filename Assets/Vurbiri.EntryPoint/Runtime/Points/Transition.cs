using System;
using UnityEngine.SceneManagement;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.EntryPoint
{
	sealed public class Transition
	{
		private static Transition s_instance;
		private static Action<int> s_onExit;

		private readonly VAction _eventExit = new();
		private readonly IEnterParam _enterParam;
		private ExitParam _exitParam;
		private IDisposable _sceneContainer;

		public static Event OnExit { [Impl(256)] get => s_instance._eventExit; }

		internal static Transition Instance { [Impl(256)] get => s_instance; }

		[Impl(256)] private Transition(IEnterParam enterParam) => _enterParam = enterParam;

		[Impl(256)] internal static void Create(Action<int> onExit, IEnterParam enterParam)
		{
			s_onExit = onExit;
			s_instance = new(enterParam);
		}

		[Impl(256)] public void Setup(IDisposable sceneContainer, int nextScene) => Setup(sceneContainer, new ExitParam(nextScene));
		[Impl(256)] public void Setup(IDisposable sceneContainer, ExitParam exitParam)
		{
			_exitParam = exitParam;
			_sceneContainer = sceneContainer;
		}

		[Impl(256)] public T GetEnterParam<T>() where T : IEnterParam => (T)_enterParam;

		[Impl(256)] public static void Exit() => s_instance.ExitInternal();
		[Impl(256)] public static void Exit(int nextScene) => Exit(new ExitParam(nextScene));
		[Impl(256)] public static void Exit(ExitParam exitParam)
		{
			s_instance._exitParam = exitParam;
			s_instance.ExitInternal();
		}

		private async void ExitInternal()
		{
			SceneManager.sceneUnloaded += OnSceneUnloaded;
			_eventExit.InvokeOneShot();
			await System.Threading.Tasks.Task.Delay((int)(2000f * UnityEngine.Time.unscaledDeltaTime) + 1);

			s_instance = new(_exitParam.EnterParam);
			s_onExit.Invoke(_exitParam.NextScene);
		}

		private void OnSceneUnloaded(Scene scene)
		{
			SceneManager.sceneUnloaded -= OnSceneUnloaded;
			_sceneContainer.Dispose();
		}
	}
}
