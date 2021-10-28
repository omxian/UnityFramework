using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapshot
{
    /// <summary>
    /// 监控数据单元，最小监控数据单位
    /// </summary>
    public struct SnapshotDataUnit
    {
        public string Name { get; private set; }
        public int Count { get; private set; }
        public int Weight { get; private set; }

        public SnapshotDataUnit(string name, int count, int weight = 1)
        {
            Name = name;
            Count = count;
            Weight = weight;
        }
    }

}
