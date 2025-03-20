//Assets\Vurbiri\Editor\EntryPoint\EntryPointExecutionOrder.cs
using UnityEditor;
using Vurbiri.EntryPoint;

namespace VurbiriEditor.EntryPoint
{
    public class EntryPointExecutionOrder
	{
        private const string PATH = "Assets/Vurbiri/Runtime/EntryPoint/Points/{0}.cs";
        private const int SCENE_ORDER = -15, PROJECT_ORDER = 5;
        
        [InitializeOnLoadMethod]
        private static void SetExecutionOrder()
        { 
            SetOrder<ASceneEntryPoint>(SCENE_ORDER);
            SetOrder<AProjectEntryPoint>(PROJECT_ORDER);

            #region Local: SetOrder(..)
            //=================================
            static void SetOrder<T>(int order)
            {
                var path = string.Format(PATH, typeof(T).Name);
                var monoImporter = (MonoImporter)AssetImporter.GetAtPath(path);
                var monoScript = monoImporter.GetScript();

                if (MonoImporter.GetExecutionOrder(monoScript) != order)
                {
                    MonoImporter.SetExecutionOrder(monoScript, order);
                    EditorUtility.DisplayDialog("EntryPointExecutionOrder", $"{monoScript.name} set execution order {order}", "OK");
                }
            }
            #endregion
        }
    }
}
