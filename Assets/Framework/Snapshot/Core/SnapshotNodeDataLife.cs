using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Snapshot
{
    /// <summary>
    /// 监控节点的生命线
    /// </summary>
    public class SnapshotNodeDataLife
    {
        public SnapshotType SnapshotType { get; private set; }
        public int Count { get => _dataList.Count; }
        public SnapshotNodeData this[int index] { get => _dataList[index]; }

        private ISnapshotNode _node;
        private List<SnapshotNodeData> _dataList = new List<SnapshotNodeData>();

        public SnapshotNodeDataLife(SnapshotType snapshotType, ISnapshotNode node)
        {
            _node = node;
            SnapshotType = snapshotType;
        }

        /// <summary>
        /// 添加一个节点数据
        /// </summary>
        /// <param name="node"></param>
        public void Add(SnapshotNodeData node)
        {
            _dataList.Add(node);
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            _dataList.Clear();
        }

        /// <summary>
        /// 拍一次快照
        /// </summary>
        /// <param name="curTime">当前时间</param>
        /// <param name="index">快照索引</param>
        public void Snapshot(int index)
        {
            var data = _node.SnapShot();
            data.SetSnapshotIndex(index);
            Add(data);
        }

        /// <summary>
        /// 获取快照数据
        /// </summary>
        /// <param name="index">快照索引</param>
        /// <returns></returns>
        public SnapshotNodeData GetSnapshotData(int index)
        {
            int count = _dataList.Count;
            for (int i = 0; i < count; i++)
            {
                if (_dataList[i].SnapshotIndex == index)
                {
                    return _dataList[i];
                }
            }
            return SnapshotNodeData.Empty;
        }

        /// <summary>
        /// 获取两个节点间数据差异
        /// </summary>
        /// <param name="oldIndex">旧的索引</param>
        /// <param name="newIndex">新的索引</param>
        /// <param name="isOnlyIncrease">是否只获取增量数据</param>
        /// <returns></returns>
        public SnapshotNodeData GetDifferent(int oldIndex, int newIndex, bool isOnlyIncrease = false)
        {
            SnapshotNodeData oldData = GetSnapshotData(oldIndex);
            SnapshotNodeData newData = GetSnapshotData(newIndex);
            if (oldData == null)
                oldData = SnapshotNodeData.Empty;
            if (newData == null)
                newData = SnapshotNodeData.Empty;
            return GetDifferent(oldData, newData, isOnlyIncrease);
        }

        /// <summary>
        /// 获取两个节点间数据差异
        /// </summary>
        /// <param name="oldData">旧的数据</param>
        /// <param name="newData">新的数据</param>
        /// <param name="isOnlyIncrease">是否只获取增量数据</param>
        /// <returns></returns>
        public SnapshotNodeData GetDifferent(SnapshotNodeData oldData, SnapshotNodeData newData, bool isOnlyIncrease = false)
        {
            List<SnapshotDataUnit> dataList = new List<SnapshotDataUnit>();
            HashSet<string> sameKey = new HashSet<string>();
            SnapshotDataUnit oldItem;
            int diff = 0;
            foreach (var newItem in newData.Data)
            {
                if (oldData.Contains(newItem.Key))
                {
                    oldItem = oldData[newItem.Key];
                    sameKey.Add(newItem.Key);
                    diff = newItem.Value.Count - oldItem.Count;
                    if (!isOnlyIncrease || diff > 0)                // 对比所有，或者只显示增量时
                    {
                        dataList.Add(new SnapshotDataUnit(newItem.Key, newItem.Value.Count - oldItem.Count));
                    }
                }
                else
                {
                    if (newItem.Value.Count > 0)
                        dataList.Add(new SnapshotDataUnit(newItem.Key, newItem.Value.Count));
                }
            }

            if (!isOnlyIncrease)
            {
                foreach (var item in oldData.Data)
                {
                    if (!sameKey.Contains(item.Key))
                    {
                        dataList.Add(new SnapshotDataUnit(item.Key, -1 * item.Value.Count));
                    }
                }
            }

            return new SnapshotNodeData(dataList);
        }

    }
}
