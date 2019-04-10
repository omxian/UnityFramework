using System.IO;
using tabtoy;
using table;
using UnityEngine;

/// <summary>
/// ���ͨ��ConfigManager.Instance.config��ȡ��������Ϣ
/// ���֮����������ط�����Ҫ��չ����չ���˴�
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
            Debug.Log("�����ļ������⣬����������!");
            return;
        }

        Table.Deserialize(config, reader);
    }
}
