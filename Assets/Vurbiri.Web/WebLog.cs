namespace Vurbiri.Web
{
    sealed public class WebLog : Log
    {
        protected override void InfoRuntime(string msg) => UtilityJS.LogJS(msg);
        protected override void WarningRuntime(string msg) => UtilityJS.LogJS(msg);
        protected override void ErrorRuntime(string msg) => UtilityJS.ErrorJS(msg);
    }
}
