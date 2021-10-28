using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Snapshot
{

    /// <summary>
    /// 监控节点数据
    /// </summary>
    public class SnapshotNodeData
    {
        readonly static public SnapshotNodeData Empty = new SnapshotNodeData();
        readonly static public SnapshotNodeData Invalid = new SnapshotNodeData(new List<SnapshotDataUnit>() { new SnapshotDataUnit("【数据非法】", -1) });

        public int SnapshotIndex { get; private set; }
        public int Count { get => _dataDic.Count; }
        public Dictionary<string, SnapshotDataUnit> Data { get => _dataDic; }
        public SnapshotDataUnit this[string key] { get => _dataDic[key]; }

        /// <summary>
        /// 监控数据列表
        /// </summary>
        private Dictionary<string, SnapshotDataUnit> _dataDic = new Dictionary<string, SnapshotDataUnit>();

        public SnapshotNodeData(List<SnapshotDataUnit> dataList = null)
        {
            _dataDic.Clear();
            if (dataList != null)
            {
                dataList.Sort((a, b) => { return (b.Weight * b.Count) - (a.Weight * a.Count); });
                int count = dataList.Count;
                for (int i = 0; i < count; i++)
                {
                    _dataDic.Add(dataList[i].Name, dataList[i]);
                }
            }
        }
        

        public SnapshotNodeData(Dictionary<string, SnapshotDataUnit> dataDic)
        {
            if (dataDic != null)
            {
                _dataDic = dataDic;
            }
        }

        /// <summary>
        /// 设置快照索引
        /// </summary>
        /// <param name="snapshotTime"></param>
        public void SetSnapshotIndex(int index)
        {
            SnapshotIndex = index;
        }

        /// <summary>
        /// 添加数据单元
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="unit">数据单元</param>
        /// <param name="cover">相同Key的话覆盖操作</param>
        public void Add(SnapshotDataUnit unit, bool cover = false)
        {
            string key = unit.Name;
            if (_dataDic.ContainsKey(key))
            {
                if (!cover)
                {
                    Debug.LogErrorFormat("尝试添加一个已存在相同Key的数据单元，且没有设置覆盖。key:{0}", key);
                    return;
                }
                else
                {
                    _dataDic[key] = unit;
                }
            }
            else
            {
                _dataDic.Add(key, unit);
            }
        }

        /// <summary>
        /// 移除数据单元
        /// </summary>
        /// <param name="key">数据单元名字</param>
        public void Remove(string key)
        {
            if (_dataDic.ContainsKey(key))
            {
                _dataDic.Remove(key);
            }
        }

        /// <summary>
        /// 移除数据单元
        /// </summary>
        /// <param name="unit">数据单元</param>
        public void Remove(SnapshotDataUnit unit)
        {
            Remove(unit.Name);
        }

        /// <summary>
        /// 清除所有
        /// </summary>
        public void Clear()
        {
            _dataDic.Clear();
        }

        /// <summary>
        /// 是否包含Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return _dataDic.ContainsKey(key);
        }

        /// <summary>
        /// 是否包含Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(SnapshotDataUnit unit)
        {
            return Contains(unit.Name);
        }

    }

}
