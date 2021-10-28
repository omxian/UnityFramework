using System.Collections.Generic;

namespace Snapshot
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public static class SnapshotConfig
    {
        public static string OutputFolder = "SnapshotOutput";

        //无效监控节点
        public static readonly ISnapshotNode InvalidNode = new InvalidNode();

        /// <summary>
        /// 创建监控字典，在此处添加检测节点
        /// </summary>
        public static readonly Dictionary<SnapshotType, ISnapshotNode> SnapshotCreateDic = new Dictionary<SnapshotType, ISnapshotNode>()
        {
            {SnapshotType.None, new InvalidNode()},
        };

        /// <summary>
        /// 获取监控节点
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ISnapshotNode GetSnapshotNode(SnapshotType type)
        {
            if (SnapshotCreateDic.ContainsKey(type))
            {
                return SnapshotCreateDic[type];
            }
            return InvalidNode;
        }
    }

    /// <summary>
    /// 监控目标类型
    /// </summary>
    public enum SnapshotType
    {
        None = 0,
        ResourceManager = 1 << 1,
        PoolManager = 1 << 2,
    }
}
