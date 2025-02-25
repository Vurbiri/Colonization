//Assets\Colonization\Scripts\EntryPoint\Project\ProjectEntryPoint.cs
using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Data;
using Vurbiri.EntryPoint;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization
{
    [DefaultExecutionOrder(5)]
    public class ProjectEntryPoint : AProjectEntryPoint
	{
		private void Start()
		{
			StartCoroutine(Run_Cn(GetComponent<ProjectInitializationData>()));
		}

		private IEnumerator Run_Cn(ProjectInitializationData data)
		{
			LoadScene loadScene = data.startScene;
			loadScene.Start();

			yield return Init_Cn(data);

			data.Dispose();

			loadScene.End();
		}

		private IEnumerator Init_Cn(ProjectInitializationData data)
		{
			Message.Log("Start Init Project");

            _servicesContainer.AddInstance(Localization.Instance).SetFiles(data.localizationFiles);

			var coroutine = _servicesContainer.AddInstance(Coroutines.Create("Project Coroutines", true));

            var ysdk = _servicesContainer.AddInstance(new YandexSDK(coroutine, data.leaderboardName));
			yield return ysdk.Init_Cn();

            _dataContainer.AddInstance(data.settings);
            _dataContainer.AddInstance(data.settingsColorScriptable.Colors);
            //Banners.InstanceF.Initialize();

            yield return CreateStorage_Cn(coroutine, ysdk, data);
            yield return YandexIsLogOn_Cn(coroutine, ysdk, data);

            _dataContainer.AddInstance(new GameSettings(_servicesContainer));

            Message.Log("End Init Project");

            #region Local: CreateStorage_Cn(..), YandexIsLogOn_Cn(..)
            //=================================
            IEnumerator CreateStorage_Cn(Coroutines coroutine, YandexSDK ysdk, ProjectInitializationData data)
			{
                IStorageService storage = null;
                yield return StartCoroutine(Storage.Create_Cn(_servicesContainer, SAVE_KEYS.PROJECT, s => storage = s));

                var projectSaveData = _dataContainer.ReplaceInstance(new ProjectSaveData(coroutine, storage));
                data.settings.Init(ysdk, projectSaveData.SettingsLoadData);
                projectSaveData.SettingsBind(data.settings);
            }
            //=================================
            IEnumerator YandexIsLogOn_Cn(Coroutines coroutine, YandexSDK ysdk, ProjectInitializationData data)
            {
                if (!ysdk.IsLogOn)
                {
                    _loadingScreen.SmoothOff_Wt();
                    yield return data.logOnPanel.TryLogOn_Cn(ysdk);
                    yield return _loadingScreen.SmoothOn_Wt();
                    if (ysdk.IsLogOn)
                        yield return CreateStorage_Cn(coroutine, ysdk, data);
                }
            }
            #endregion
        }
	}
}
