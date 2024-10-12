using System.Collections;
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

            yield return StartCoroutine(Init_Coroutine(data));

            Destroy(data);

            yield return loadScene.End();

            FindAndRunScene();
        }

        private IEnumerator Init_Coroutine(ProjectInitializationData data)
        {
            //Message.Log("Start LoadingPreGame");

            Language localization = Language.Instance;
            if (!Language.IsValid)
                Message.Error("Error loading Localization!");

            SettingsData settings = SettingsData.Instance;

            YandexSDK ysdk = new(_servicesContainer, data.leaderboardName);
            yield return StartCoroutine(ysdk.Init_Coroutine());
            _servicesContainer.AddInstance(ysdk);

            //Banners.InstanceF.Initialize();

            yield return StartCoroutine(CreateStorages_Coroutine(data.keySaveProject, data.gameplayDefaultData));

            if (!ysdk.IsLogOn)
            {
                _loadingScreen.SmoothOff_Wait();
                yield return StartCoroutine(data.logOnPanel.TryLogOn_Coroutine(ysdk));
                yield return _loadingScreen.SmoothOn_Wait();
                if (ysdk.IsLogOn)
                    yield return StartCoroutine(CreateStorages_Coroutine(data.keySaveProject, data.gameplayDefaultData));
            }

            //Message.Log("End LoadingPreGame");

            #region Local: CreateStorages_Coroutine()
            //==========================================
            IEnumerator CreateStorages_Coroutine(string key, ProjectInitializationData.GameplayDefaultData data)
            {
                _servicesContainer.Remove<IStorageService>();
                IStorageService storage = new Storage();
                _servicesContainer.AddInstance(storage);

                if (storage.Init(_servicesContainer))
                {
                    bool result = false;
                    yield return StartCoroutine(storage.Load_Coroutine(key, (b) => result = b));
                    Message.Log(result ? "Сохранение загружено" : "Сохранение не найдено");
                }

                 settings.Init(_servicesContainer);

                _dataContainer.Remove<GameplaySettingsData>();
                _dataContainer.AddInstance<GameplaySettingsData>(new(_servicesContainer, data.keySave, data.chanceWater));
            }
            #endregion
        }

        //private void OnDisable() => YandexSDK.Instance.LoadingAPI_Ready();


    }
}
