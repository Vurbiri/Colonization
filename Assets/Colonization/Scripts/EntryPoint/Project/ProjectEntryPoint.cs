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

            loadScene.End();
        }

        private IEnumerator Init_Coroutine(ProjectInitializationData data)
        {
            //Message.Log("Start LoadingPreGame");

            if (!Language.IsValid)
                Message.Error("Error loading Localization!");

            YandexSDK ysdk = new(_servicesContainer, data.leaderboardName);
            yield return StartCoroutine(ysdk.Init_Coroutine());
            _servicesContainer.AddInstance(ysdk);

            //Banners.InstanceF.Initialize();

            yield return StartCoroutine(CreateStorages_Coroutine(data.defaultProfile));
            yield return StartCoroutine(YandexIsLogOn_Coroutine(ysdk, data.logOnPanel, data.defaultProfile));

            _dataContainer.AddInstance<GameplaySettingsData>(new(_servicesContainer, data.gameplayDefaultData));

            //Message.Log("End LoadingPreGame");
        }

        private IEnumerator CreateStorages_Coroutine(SettingsData.Profile defaultProfile)
        {
            _servicesContainer.Remove<IStorageService>();
            IStorageService storage = new Storage();
            _servicesContainer.AddInstance(storage);

            if (storage.Init(_servicesContainer))
            {
                bool result = false;
                yield return StartCoroutine(storage.Load_Coroutine(SAVE_KEYS.PROJECT, (b) => result = b));
                Message.Log(result ? "Сохранение загружено" : "Сохранение не найдено");
            }

            _dataContainer.Remove<SettingsData>();
            _dataContainer.AddInstance(new SettingsData(_servicesContainer, defaultProfile));
        }

        private IEnumerator YandexIsLogOn_Coroutine(YandexSDK ysdk, LogOnPanel logOnPanel, SettingsData.Profile defaultProfile)
        {
            if (!ysdk.IsLogOn)
            {
                _loadingScreen.SmoothOff_Wait();
                yield return StartCoroutine(logOnPanel.TryLogOn_Coroutine(ysdk));
                yield return _loadingScreen.SmoothOn_Wait();
                if (ysdk.IsLogOn)
                    yield return StartCoroutine(CreateStorages_Coroutine(defaultProfile));
            }
        }

        //private void OnDisable() => YandexSDK.Instance.LoadingAPI_Ready();
    }
}
