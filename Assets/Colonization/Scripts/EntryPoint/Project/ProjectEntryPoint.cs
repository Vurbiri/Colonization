//Assets\Colonization\Scripts\EntryPoint\Project\ProjectEntryPoint.cs
using System.Collections;
using Vurbiri.Colonization.Data;
using Vurbiri.EntryPoint;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class ProjectEntryPoint : AProjectEntryPoint
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

			var coroutine = _servicesContainer.AddInstance(Coroutines.Create("Project Coroutine", true));

            var ysdk = _servicesContainer.AddInstance(new YandexSDK(coroutine, data.leaderboardName));
			yield return ysdk.Init_Cn();

            _dataContainer.AddInstance(data.settings);
            _dataContainer.AddInstance(data.settingsColorScriptable.Colors);
            //Banners.InstanceF.Initialize();

            yield return CreateStorage_Cn(ysdk, data);
            yield return YandexIsLogOn_Cn(ysdk, data);

            _dataContainer.AddInstance(new GameSettings(_servicesContainer));

            Message.Log("End Init Project");

            #region Local: CreateStorage_Cn(..), YandexIsLogOn_Cn(..)
            //=================================
            IEnumerator CreateStorage_Cn(YandexSDK ysdk, ProjectInitializationData data)
            {
                yield return StartCoroutine(Storage.Create_Cn(_servicesContainer, SAVE_KEYS.PROJECT, storage =>
                {
                    var projectSaveData = _dataContainer.ReplaceInstance(new ProjectSaveData(storage));
                    data.settings.Init(ysdk, projectSaveData);
                }));
            }
            //=================================
            IEnumerator YandexIsLogOn_Cn(YandexSDK ysdk, ProjectInitializationData data)
            {
                if (!ysdk.IsLogOn)
                {
                    _loadingScreen.SmoothOff_Wait();
                    yield return data.logOnPanel.TryLogOn_Cn(ysdk);
                    yield return _loadingScreen.SmoothOn_Wait();
                    if (ysdk.IsLogOn)
                        yield return CreateStorage_Cn(ysdk, data);
                }
            }
            #endregion
        }
	}
}
