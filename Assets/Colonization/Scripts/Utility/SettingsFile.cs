namespace Vurbiri.Colonization
{
    public class SettingsFile
	{
        public const string FOLDER = "Settings/";

        [System.Runtime.CompilerServices.MethodImpl(256)]
        public static T Load<T>()
        {
            var type = typeof(T);
            return (T)Vurbiri.Storage.LoadObjectFromJsonResource(string.Concat(FOLDER, type.Name), type); ;
        }
    }
}
