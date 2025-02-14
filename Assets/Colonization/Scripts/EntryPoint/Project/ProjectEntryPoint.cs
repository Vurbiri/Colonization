//Assets\Colonization\Scripts\EntryPoint\Project\ProjectEntryPoint.cs
using System.Collections;
using Vurbiri.Colonization.Data;
using Vurbiri.EntryPoint;
using Vurbiri.Localization;

namespace Vurbiri.Colonization
{
    public class ProjectEntryPoint : AProjectEntryPoint
	{
		private void Start()
		{
			StartCoroutine(Run_Coroutine(GetComponent<ProjectInitializationData>()));
		}

		private IEnumerator Run_Coroutine(ProjectInitializationData data)
		{
			LoadScene loadScene = data.startScene;
			loadScene.Start();

			yield return Init_Coroutine(data);

			data.Dispose();

			loadScene.End();
		}

		private IEnumerator Init_Coroutine(ProjectInitializationData data)
		{
			Message.Log("Start Init Project");

            using SettingsScriptable settings = ProjectSettingsScriptable.GetCurrentSettings();

            if (!_servicesContainer.AddInstance(new Language(settings?.LoadFiles)).IsValid)
				Message.Error("Error loading Localization!");

            _dataContainer.AddInstance(data.settingsColorScriptable.Colors);

            var ysdk = _servicesContainer.AddInstance(new YandexSDK(_servicesContainer, data.leaderboardName));
			yield return ysdk.Init_Coroutine();
			
			//Banners.InstanceF.Initialize();

			yield return CreateStorages_Coroutine(data.defaultProfile);
			yield return YandexIsLogOn_Coroutine(ysdk, data.logOnPanel, data.defaultProfile);

			_dataContainer.AddInstance(new GameplaySettingsData(_servicesContainer));

            Message.Log("End Init Project");
		}

		private IEnumerator CreateStorages_Coroutine(SettingsData.Profile defaultProfile)
		{
            yield return StartCoroutine(Storage.Create_Coroutine(_servicesContainer, SAVE_KEYS.PROJECT));
			_dataContainer.ReplaceInstance(new SettingsData(_servicesContainer, defaultProfile));
		}

		private IEnumerator YandexIsLogOn_Coroutine(YandexSDK ysdk, LogOnPanel logOnPanel, SettingsData.Profile defaultProfile)
		{
			if (!ysdk.IsLogOn)
			{
				_loadingScreen.SmoothOff_Wait();
				yield return logOnPanel.TryLogOn_Coroutine(ysdk);
				yield return _loadingScreen.SmoothOn_Wait();
				if (ysdk.IsLogOn)
					yield return CreateStorages_Coroutine(defaultProfile);
			}
		}
	}
}
