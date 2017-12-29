using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Notify;
/// <summary>
/// 负责检查资源更新
/// 负责首次打开时将包内资源移到包外
/// </summary>
class UpdateManager : MonoSingleton<ResourceManager>
{
    private UpdateManager(){ }
    //如果包外存在这个文件，则可以认为已将包内资源移到包外
    private const string finishFileName = "finish";
    public override void StartUp()
    {
        NotifyManager.Instance.AddNotify(NotifyIds.FRAMEWORK_CHECK_RESOURCE, CheckResource);
    }

    private void CheckResource(NotifyArg args)
    {

    }

    /// <summary>
    /// 完成资源转移操作之后写该文件
    /// </summary>
    private void WriteFinishFile()
    {

    }
}
