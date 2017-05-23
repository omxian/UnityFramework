using System;
using System.Diagnostics;

/// <summary>
/// 简单计时器
/// 使用方式 using (new EasyWatch("test1",1000))　{//code}
/// </summary>
public class EasyWatch : IDisposable
{
    private string name;
    private int count;
    private Stopwatch watch;

    public EasyWatch(string name, int count)
    {
        this.name = name;
        this.count = count;
        watch = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        watch.Stop();
        UnityEngine.Debug.Log(string.Format("{0} 总耗时:{1} 平均耗时:{2} 运行次数{3}", 
            name, 
            watch.ElapsedMilliseconds, 
            watch.ElapsedMilliseconds / count,
            count
            ));
    }
}
