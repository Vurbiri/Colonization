
public partial class YMoney : ASingleton<YMoney>
{

#if !UNITY_EDITOR
    public bool IsInitialize => IsInitializeJS();

    public void ShowBannerAdv() => ShowBannerAdvJS();
    public void HideBannerAdv() => HideBannerAdvJS();
#else
    public bool IsInitialize => true;

    public void ShowBannerAdv() { }
    public void HideBannerAdv() { }
#endif
}
