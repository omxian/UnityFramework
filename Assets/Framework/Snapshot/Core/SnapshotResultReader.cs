using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Snapshot
{
    public class SnapshotResultReader
    {
        static private string _increaseColor = "#ff0000";
        static private string _equalColor = "#888888";
        static private string _reduceColor = "#00ff00";
        static private string _splitColor = "#ffff00";

        /// <summary>
        /// 是否为数据单元字符加色彩标签
        /// </summary>
        static public bool IsColorful { get; set; }

        /// <summary>
        /// 获取单元数据结果，如果开启白名单，白名单内的输出为空字符串
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        static public string GetDataUnit(SnapshotDataUnit unit)
        {
            string countStr = $"{unit.Name},{unit.Count,-25}";
            if (IsColorful)
            {
                countStr = string.Format("<color={0}>{1}</color>",
                    unit.Count > 0 ? _increaseColor : (unit.Count == 0 ? _equalColor : _reduceColor), countStr);
            }
            return countStr;
        }

        /// <summary>
        /// 获取监控节点数据结果
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public string GetNodeData(SnapshotNodeData data)
        {
            StringBuilder result = new StringBuilder();
            int count = data.Count;
            SnapshotDataUnit unit;
            foreach (var item in data.Data)
            {
                unit = item.Value;
                result.AppendLine(GetDataUnit(unit));
            }
            return result.ToString();
        }

        /// <summary>
        /// 获取监控生命线结果
        /// </summary>
        /// <param name="life"></param>
        /// <returns></returns>
        static public string GetLifeData(SnapshotNodeDataLife life)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(string.Format("===========监控类型：{0}===========", life.SnapshotType));
            int count = life.Count;
            for (int i = 0; i < count; i++)
            {
                result.AppendLine(GetNodeData(life[i]));
                result.AppendLine("==================================");
            }
            return result.ToString();
        }

        /// <summary>
        /// 获取所有生命在index索引时的数据
        /// </summary>
        /// <param name="lifeList"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        static public string GetAllLifeDataAtIndex(List<SnapshotNodeDataLife> lifeList, SnapshotInfo info)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"===========【{info}】 ===========");
            int count = lifeList.Count;
            for (int i = 0; i < count; i++)
            {
                result.AppendLine(string.Format("===========监控类型：{0}===========", lifeList[i].SnapshotType));
                result.Append($"{GetNodeData(lifeList[i].GetSnapshotData(info.index))}");
                result.AppendLine(string.Format("=================================="));
                result.AppendLine("");
            }
            return result.ToString();
        }

        /// <summary>
        /// 获取所有生命线在oldIndex到newIndex的差异
        /// </summary>
        /// <param name="lifeList">生命线列表</param>
        /// <param name="oldInfo">旧索引</param>
        /// <param name="newInfo">新索引</param>
        /// <param name="isOnlyIncrease">是否只打印增量</param>
        /// <param name="isColorful">是否要打印带色彩</param>
        /// <returns></returns>
        static public string GetAllLifeDataDifferent(List<SnapshotNodeDataLife> lifeList, SnapshotInfo oldInfo, SnapshotInfo newInfo, bool isOnlyIncrease)
        {
            StringBuilder result = new StringBuilder();
            bool bInit = false;
            int count = lifeList.Count;
            for (int i = 0; i < count; i++)
            {
                var nodeData = lifeList[i].GetDifferent(oldInfo.index, newInfo.index, isOnlyIncrease);
                if (nodeData.Count == 0)
                {
                    continue;
                }
                if(!bInit)
                {
                    result.AppendLine(string.Format("=======【{0}】和【{1}】 的对比结果=======", oldInfo, newInfo));
                    bInit = true;
                }
                result.AppendLine(string.Format("===========监控类型：{0}===========", lifeList[i].SnapshotType));
                result.Append($"{GetNodeData(nodeData)}");
                result.AppendLine(string.Format("=================================="));
                result.AppendLine("");
            }
            if(bInit)
            {
                return result.ToString();
            }
            return null;
        }
    }
}
