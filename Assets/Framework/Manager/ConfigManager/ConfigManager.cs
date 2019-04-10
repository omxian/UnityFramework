using System.IO;
using tabtoy;
using table;
using UnityEngine;

/// <summary>
/// 外界通过ConfigManager.Instance.config获取到配置信息
/// 如果之后有配置相关方法需要扩展，扩展到此处
/// </summary>
public class ConfigManager : MonoSingleton<ConfigManager>
{
    private ConfigManager() { }

    public Table config;

    public override void StartUp()
    {
        base.StartUp();
        LoadConfig();
    }

    private void LoadConfig()
    {
        byte[] configBytes = ResourceManager.Instance.LoadConfig();
        Stream stream = new MemoryStream(configBytes);
        var reader = new DataReader(stream);
        config = new Table();

        var result = reader.ReadHeader(config.GetBuildID());
        if (result != FileState.OK)
        {
            Debug.Log("配置文件有问题，请重新生成!");
            return;
        }

        Table.Deserialize(config, reader);
    }
}
