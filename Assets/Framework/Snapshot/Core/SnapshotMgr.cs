using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snapshot
{
    //快照信息
    public class SnapshotInfo
    {
        public int index;
        public DateTime time;
        public string desc;

        static readonly public SnapshotInfo Empty = new SnapshotInfo();

        public override string ToString()
        {
            return $"index：{index}， desc：{desc}， time:{time}";
        }
    }

    public class SnapshotMgr: ISnapshotMgr
    {
        /// <summary>
        /// 直接根据类型获取生命线，包括缓存内数据
        /// </summary>
        /// <param name="snapshotType">监控类型</param>
        /// <returns></returns>
        public SnapshotNodeDataLife this[SnapshotType snapshotType] { get => _lifeDic.ContainsKey(snapshotType) ? _lifeDic[snapshotType] : TryGetLife(_lifeCache, snapshotType); }
        public int SnapshotCount { get => _lifeDic.Count; }
        public int SnapshotIndex { get; private set; }

        // 存放当前要快照的生命线
        private Dictionary<SnapshotType, SnapshotNodeDataLife> _lifeDic = new Dictionary<SnapshotType, SnapshotNodeDataLife>();
        // 存放当前不需要快照的生命线，但还是可以拿到之前的数据
        private Dictionary<SnapshotType, SnapshotNodeDataLife> _lifeCache = new Dictionary<SnapshotType, SnapshotNodeDataLife>();
        // 快照索引 对应信息
        private Dictionary<int, SnapshotInfo> _snapshotInfoDic = new Dictionary<int, SnapshotInfo>();

        public SnapshotMgr()
        {
            SnapshotIndex = -1;
        }

        /// <summary>
        /// 尝试获取数据，不报错
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private SnapshotNodeDataLife TryGetLife(Dictionary<SnapshotType, SnapshotNodeDataLife> dic, SnapshotType key)
        {
            SnapshotNodeDataLife result;
            dic.TryGetValue(key, out result);
            return result;
        }

        /// <summary>
        /// 添加监控
        /// </summary>
        /// <param name="snapshotType"></param>
        public void AddSnapshot(SnapshotType snapshotType)
        {
            if (!_lifeDic.ContainsKey(snapshotType))
            {
                SnapshotNodeDataLife life = null;
                if (_lifeCache.ContainsKey(snapshotType))        // 从缓存里面取，包含了以前的索引数据
                {
                    life = _lifeCache[snapshotType];
                    _lifeCache.Remove(snapshotType);
                }
                else
                {
                    life = new SnapshotNodeDataLife(snapshotType, SnapshotConfig.GetSnapshotNode(snapshotType));
                }
                _lifeDic.Add(snapshotType, life);
            }
        }

        /// <summary>
        /// 移除监控，不是真的删，而是存到另一个字典。
        /// </summary>
        /// <param name="snapshotType"></param>
        public void RemoveSnapshot(SnapshotType snapshotType)
        {
            if (_lifeDic.ContainsKey(snapshotType))
            {
                if (_lifeCache.ContainsKey(snapshotType))
                {
                    _lifeCache.Remove(snapshotType);
                }
                _lifeCache.Add(snapshotType, _lifeDic[snapshotType]);    // 添加缓存里面，后面要回之前的索引数据可以拿回来
                _lifeDic.Remove(snapshotType);
            }
        }

        /// <summary>
        /// 清除所有监控
        /// </summary>
        public void ClearAll()
        {
            SnapshotIndex = -1;
            _lifeDic.Clear();
            _lifeCache.Clear();
            _snapshotInfoDic.Clear();
        }

        /// <summary>
        /// 打印一次快照，返回打印次数索引
        /// </summary>
        /// <returns></returns>
        public int Snapshot(string desc)
        {
            ++SnapshotIndex;
            _snapshotInfoDic.Add(SnapshotIndex, new SnapshotInfo { index = SnapshotIndex, time = DateTime.Now, desc = desc });
            foreach (var item in _lifeDic)
            {
                item.Value.Snapshot(SnapshotIndex);
            }
            return SnapshotIndex;
        }

        /// <summary>
        /// 快照，支持指定监控类型
        /// </summary>
        /// <param name="snapshotType"></param>
        /// <returns></returns>
        public int Snapshot(SnapshotType snapshotType, string desc)
        {
            RefreshSnapshotType(snapshotType);
            return Snapshot(desc);
        }

        /// <summary>
        /// 刷新监控器类型
        /// </summary>
        /// <param name="snapshotType"></param>
        public void RefreshSnapshotType(SnapshotType snapshotType)
        {
            foreach (SnapshotType type in Enum.GetValues(typeof(SnapshotType)))
            {
                if (SnapshotUtil.ContainType(snapshotType, type))
                {
                    AddSnapshot(type);
                }
                else
                {
                    RemoveSnapshot(type);
                }
            }
        }

        /// <summary>
        /// 获取快照结果
        /// </summary>
        /// <returns></returns>
        public SnapshotNodeData GetSnapshotResult(SnapshotType type, int index)
        {
            if (!_lifeDic.ContainsKey(type))
            {
                Debug.LogErrorFormat("【【获取快照结果失败！原因：不在监控的监控类型：{0}】】", type);
                return null;
            }
            return _lifeDic[type].GetSnapshotData(index);
        }

        /// <summary>
        /// 获取监控生命线
        /// </summary>
        /// <param name="type">监控类型</param>
        /// <returns></returns>
        public SnapshotNodeDataLife GetLife(SnapshotType type, bool isIncludeCache = true)
        {
            return isIncludeCache ? this[type] : TryGetLife(_lifeDic, type);
        }

        /// <summary>
        /// 通过掩码枚举，获取监控生命线
        /// </summary>
        /// <param name="mask">监控类型</param>
        /// <param name="isIncludeCache">包括缓存内的</param>
        /// <returns></returns>
        public List<SnapshotNodeDataLife> GetLifeList(SnapshotType mask, bool isIncludeCache = true)
        {
            var lifeList = new List<SnapshotNodeDataLife>();

            foreach (SnapshotType type in Enum.GetValues(typeof(SnapshotType)))
            {
                if (SnapshotUtil.ContainType(mask, type))
                {
                    var life = GetLife(type, isIncludeCache);
                    if (life != null)
                    {
                        lifeList.Add(life);
                    }
                }
            }
            return lifeList;
        }

        /// <summary>
        /// 获取所有监控生命线
        /// </summary>
        /// <param name="isIncludeCache">包括缓存内的</param>
        /// <returns></returns>
        public List<SnapshotNodeDataLife> GetAllLifeList(bool isIncludeCache = true)
        {
            var lifeList = new List<SnapshotNodeDataLife>();
            foreach (var item in _lifeDic)
            {
                lifeList.Add(item.Value);
            }
            if (isIncludeCache)
            {
                foreach (var item in _lifeCache)
                {
                    lifeList.Add(item.Value);
                }
            }
            return lifeList;
        }

        /// <summary>
        /// 快照信息
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SnapshotInfo GetSnapshotInfo(int index)
        {
            return _snapshotInfoDic.ContainsKey(index) ? _snapshotInfoDic[index] : SnapshotInfo.Empty;
        }
    }
}