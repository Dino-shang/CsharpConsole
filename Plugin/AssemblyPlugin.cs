using System.Reflection;
using System.Runtime.Loader;

namespace ConsoleTest.Plugin;

public class AssemblyPlugin
{
    // 创建自定义的json插件，使用 newtonsoft.json 的dll
    class JsonLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public JsonLoadContext(string assemblyPath) : base(isCollectible: true)
        {
            _resolver = new AssemblyDependencyResolver(assemblyPath);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath == null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null; // 让默认上下文来处理
        }
    }

    public static void LoadNewtonSoftJsonPlugin()
    {
        // string assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Newtonsoft.Json.dll");
        // Assembly assembly = Assembly.LoadFrom(assemblyPath);
        string pluginPath = "C:\\projects\\Csharp\\C#项目\\控制台应用\\Console_Style\\Console_Style\\bin\\Debug\\net8.0Newtonsoft.Json.dll";

        WeakReference weakReference = Execute(pluginPath);
        
        // 触发GC
        GC.Collect();
        GC.WaitForPendingFinalizers();

        // 检查上下文是否已被卸载
        Console.WriteLine("\n-----------------------------");
        Console.WriteLine($"上下文是否被卸载？: {!weakReference.IsAlive}");
        Console.WriteLine("-----------------------------");
        
    }
    
    private static WeakReference Execute(string pluginPath)
    {
        // 2. 创建一个可回收的上下文实例
        var loadContext = new JsonLoadContext(pluginPath);
        WeakReference contextWeakRef = new WeakReference(loadContext);

        try
        {
            // 加载 Newtonsoft.Json.dll
            Assembly jsonAssembly = loadContext.LoadFromAssemblyPath(pluginPath);
            
            // 获取 JsonConvert 类型
            Type jsonConvertType = jsonAssembly.GetType("Newtonsoft.Json.JsonConvert");

            // 定义要解析的 JSON 字符串
            string jsonString = @"{ ""name"": ""Lemon"", ""app"": ""LemonApp"" }";
            Console.WriteLine($"原始 JSON 字符串: {jsonString}");
            
            // 调用 DeserializeObject 方法
            MethodInfo deserializeMethod = jsonConvertType.GetMethod("DeserializeObject", new Type[] { typeof(string) });
            object jsonObject = deserializeMethod.Invoke(null, new object[] { jsonString });

            // 将结果转换为动态类型并访问其属性
            dynamic dynamicObject = jsonObject;
            string name = dynamicObject.name;
            string app = dynamicObject.app;

            Console.WriteLine("-----------------------------");
            Console.WriteLine("成功调用 Newtonsoft.Json 方法");
            Console.WriteLine($"解析结果 - name: {name}, app: {app}");
            Console.WriteLine("-----------------------------");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生错误: {ex.Message}");
        }

        return contextWeakRef;
    }
}