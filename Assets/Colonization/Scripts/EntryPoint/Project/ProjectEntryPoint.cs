//Assets\Colonization\Scripts\EntryPoint\Project\ProjectEntryPoint.cs
using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Data;
using Vurbiri.EntryPoint;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization
{
    [DefaultExecutionOrder(10)]
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

            _servicesContainer.AddInstance(Localization.Instance).SetFiles(data.localizationFiles);

			var coroutine = _servicesContainer.AddInstance(Coroutines.Create("Project Coroutines", true));

            var ysdk = _servicesContainer.AddInstance(new YandexSDK(coroutine, data.leaderboardName));
			yield return ysdk.Init_Coroutine();

            _dataContainer.AddInstance(data.settings);
            _dataContainer.AddInstance(data.settingsColorScriptable.Colors);
            //Banners.InstanceF.Initialize();

            yield return CreateStoroge_Coroutine(coroutine, ysdk, data);
            yield return YandexIsLogOn_Coroutine(coroutine, ysdk, data);

            _dataContainer.AddInstance(new GameSettings(_servicesContainer));

            Message.Log("End Init Project");

            #region Local: LoadData_Coroutine(..), YandexIsLogOn_Coroutine(..)
            //=================================
            IEnumerator CreateStoroge_Coroutine(Coroutines coroutine, YandexSDK ysdk, ProjectInitializationData data)
			{
                IStorageService storage = null;
                yield return StartCoroutine(Storage.Create_Coroutine(_servicesContainer, SAVE_KEYS.PROJECT, s => storage = s));

                var projectSaveData = _dataContainer.ReplaceInstance(new ProjectSaveData(coroutine, storage));
                data.settings.Init(ysdk, projectSaveData.SettingsLoadData);
                projectSaveData.SettingsBind(data.settings);
            }
            //=================================
            IEnumerator YandexIsLogOn_Coroutine(Coroutines coroutine, YandexSDK ysdk, ProjectInitializationData data)
            {
                if (!ysdk.IsLogOn)
                {
                    _loadingScreen.SmoothOff_Wait();
                    yield return data.logOnPanel.TryLogOn_Coroutine(ysdk);
                    yield return _loadingScreen.SmoothOn_Wait();
                    if (ysdk.IsLogOn)
                        yield return CreateStoroge_Coroutine(coroutine, ysdk, data);
                }
            }
            #endregion
        }
	}
}
