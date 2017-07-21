using System;
/// <summary>
/// 所有的对象池的对象都必须继承此类
/// </summary>
public interface IPoolable : IDisposable
{
    /// <summary>
    /// 重新初始化
    /// </summary>
    void Reset();
}
