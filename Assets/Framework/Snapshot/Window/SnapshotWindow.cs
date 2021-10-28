using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Threading;
using System.Text;
using UnityEngine.Profiling;
using System.Reflection;

namespace Snapshot
{

    public class SnapshotWindow : EditorWindow
    {
        static readonly private SnapshotType AllSnapshotType = (SnapshotType)(-1);

        private SnapshotMgr _snapshotMgr = new SnapshotMgr();
        private SnapshotType _selectedType = AllSnapshotType;
        private SnapshotType _oldSelectedType = 0;
        private bool _isIncrease;
        private Vector2 _scrollViewPos;
        private string _temAddKey;
        private int _temAddCount;
        private string _snapshotDesc;
        private Dictionary<string, bool> _showBoxDic = new Dictionary<string, bool>();

        [MenuItem("Tools/监控器")]
        static public void ShowWindow()
        {
            EditorWindow window = GetWindow<SnapshotWindow>("监控器");
        }


        private void DrawBox(string label, Action customDraw, bool isDefualtShow = false)
        {
            if (!_showBoxDic.ContainsKey(label))
            {
                _showBoxDic.Add(label, isDefualtShow);
            }
            using (var area = new GUILayout.VerticalScope(new GUIStyle("FrameBox")))
            {
                _showBoxDic[label] = EditorGUILayout.Foldout(_showBoxDic[label], label);
                if (_showBoxDic[label])
                {
                    customDraw.Invoke();
                }
            }
        }
        private void OnEnable()
        {
            _oldSelectedType = 0;
            _outputCurIndex = 0;
            _outputOldIndex = 0;
            _outputNewIndex = 0;
            _snapshotDesc = null;
            _oldInputTabOption = -1;
            _snapshotMgr.RefreshSnapshotType(_selectedType);
        }

        private void OnGUI()
        {
            _scrollViewPos = GUILayout.BeginScrollView(_scrollViewPos);
            var lastSnapshotInfo = GetSnapshotInfo(_snapshotMgr.SnapshotIndex);
            GUILayout.Label(string.Format("当前快照索引：{0,-20} 快照时间：{1,-30}  快照描述：{2}", _snapshotMgr.SnapshotIndex, lastSnapshotInfo.time, lastSnapshotInfo.desc));

            DrawSnapshotTab();

            GUILayout.EndScrollView();
        }

        /// <summary>
        /// 获取快照信息
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private SnapshotInfo GetSnapshotInfo(int index)
        {
            return _snapshotMgr.GetSnapshotInfo(index);
        }

        #region 选择监控类型
        private int _inputTabOption = 0;
        private int _oldInputTabOption = -1;

        /// <summary>
        /// 绘制输入选择
        /// </summary>
        private void DrawSnapshotTab()
        {
            using (var scope = new GUILayout.VerticalScope("Box"))
            {

                _oldInputTabOption = _inputTabOption;
                DrawAPPSnapshot();
                if (_oldInputTabOption != _inputTabOption)
                    _snapshotMgr.RefreshSnapshotType(_selectedType);
            }
        }

        /// <summary>
        /// 应用监控 
        /// </summary>
        private void DrawAPPSnapshot()
        {
            DrawAppInput();
            DrawBox("输出数据", DrawOutput, true);
            DrawBox("其他", DrawOthers);
        }

        /// <summary>
        /// 应用层输入
        /// </summary>
        private void DrawAppInput()
        {
            _selectedType = (SnapshotType)EditorGUILayout.EnumFlagsField("监控类型", _selectedType);
            if (_oldSelectedType != _selectedType)
                _snapshotMgr.RefreshSnapshotType(_selectedType);
            _oldSelectedType = _selectedType;
            GUILayout.Label("监控器数量：" + _snapshotMgr.SnapshotCount);
            if (Application.isPlaying)
                DrawSnapshot();
            else
                EditorGUILayout.HelpBox("只能在游戏状态拍快照！", MessageType.Error);
        }


        #endregion

        #region 拍快照
        private void DrawSnapshot()
        {
            string curSnapshotDescTip = $"索引'{(_snapshotMgr.SnapshotIndex + 1)}'的快照描述";
            if (string.IsNullOrEmpty(_snapshotDesc))
            {
                _snapshotDesc = curSnapshotDescTip;
            }
            _snapshotDesc = EditorGUILayout.TextField(curSnapshotDescTip, _snapshotDesc);
            if (GUILayout.Button($"快照来一个，索引：{_snapshotMgr.SnapshotIndex + 1}"))
            {
                int curIdx = _snapshotMgr.Snapshot(_snapshotDesc);
                _snapshotDesc = string.Empty;
                _outputCurIndex = curIdx;
                Debug.LogFormat("拍了一个快照，索引：{0}", curIdx);
            }
        }
        #endregion

        #region 输出数据
        private SnapshotType _outputSelectedType = AllSnapshotType;
        private int _outputCurIndex = 0;
        private int _outputOldIndex = 0;
        private int _outputNewIndex = 0;
        private bool _outputIsIncrease = true;
        private int _outputTabOption = 0;
        /// <summary>
        /// 绘制导出编辑
        /// </summary>
        private void DrawOutput()
        {
            using (var scope = new GUILayout.HorizontalScope())
            {
                SnapshotConfig.OutputFolder = EditorGUILayout.TextField("输出目录", SnapshotConfig.OutputFolder);
                if (GUILayout.Button("浏览"))
                {
                    SnapshotConfig.OutputFolder = EditorUtility.OpenFolderPanel("监控数据输出目录", SnapshotConfig.OutputFolder, "");
                }
            }
            using (var scope = new GUILayout.VerticalScope("Box"))
            {
                _outputTabOption = GUILayout.Toolbar(_outputTabOption, new string[] { "输出快照", "输出对比" });
                EditorGUILayout.Space();
                _outputSelectedType = (SnapshotType)EditorGUILayout.EnumFlagsField("输出监控类型", _outputSelectedType);
                switch (_outputTabOption)
                {
                    case 0:
                        _outputCurIndex = ShowIntPopup("数据索引", _outputCurIndex, 0, _snapshotMgr.SnapshotIndex + 1);
                        break;
                    case 1:
                        using (var scope2 = new GUILayout.HorizontalScope())
                        {
                            _outputOldIndex = ShowIntPopup("旧数据索引", _outputOldIndex, 0, _snapshotMgr.SnapshotIndex + 1);
                            _outputNewIndex = ShowIntPopup("新数据索引", _outputNewIndex, _outputOldIndex + 1, _snapshotMgr.SnapshotIndex - _outputOldIndex);
                        }
                        _outputIsIncrease = GUILayout.Toggle(_outputIsIncrease, "是否只打印增量");

                        break;
                }
                using (var scope3 = new GUILayout.HorizontalScope())
                {
                    DrawOutputBtn();
                }
            }
        }

        /// <summary>
        /// 绘制输出按钮
        /// </summary>
        private void DrawOutputBtn()
        {
            if (GUILayout.Button("输出到控制台"))
            {
                string output = GetOutputStr(true);
                Debug.Log(output);
            }
            if (GUILayout.Button("输出到输出目录"))
            {
                string output = GetOutputStr(false);
                if (!Directory.Exists(SnapshotConfig.OutputFolder))
                {
                    Directory.CreateDirectory(SnapshotConfig.OutputFolder);
                }
                //else
                //{
                var path = Path.Combine(SnapshotConfig.OutputFolder, GetOutputFileName(_outputTabOption == 1));
                SnapshotUtil.WriteToFile(path, output, $"输出监控数据成功：{path}");
                //}
            }
        }


        List<int> _temIntList = new List<int>();
        List<string> _temIntStrList = new List<string>();
        /// <summary>
        /// 显示Int下拉列表
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="cur">当前索引</param>
        /// <param name="start">起始值</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        private int ShowIntPopup(string label, int cur, int start, int length)
        {
            _temIntList.Clear();
            _temIntStrList.Clear();
            for (int i = start; i < start + length; i++)
            {
                _temIntList.Add(i);
                _temIntStrList.Add($"{i.ToString()}：{_snapshotMgr.GetSnapshotInfo(i).desc}");
            }
            int result = EditorGUILayout.IntPopup(label, cur, _temIntStrList.ToArray(), _temIntList.ToArray());
            if (result < start || result >= start + length)
            {
                result = -1;
            }
            return result;
        }

        /// <summary>
        /// 获取输出字符串，控制台输出变化量会带颜色
        /// </summary>
        /// <param name="isConsole">是否是控制台输出</param>
        /// <returns></returns>
        private string GetOutputStr(bool isConsole)
        {
            string output = "【无效输出】";
            var lifeList = _snapshotMgr.GetLifeList(_outputSelectedType);
            int count = lifeList.Count;
            switch (_outputTabOption)
            {
                case 0:
                    if (_outputCurIndex != -1)
                    {
                        SnapshotResultReader.IsColorful = false;
                        output = SnapshotResultReader.GetAllLifeDataAtIndex(lifeList, GetSnapshotInfo(_outputCurIndex));
                    }
                    break;
                case 1:
                    if (_outputOldIndex != -1 && _outputNewIndex != -1)
                    {
                        SnapshotResultReader.IsColorful = isConsole;
                        output = SnapshotResultReader.GetAllLifeDataDifferent(lifeList, GetSnapshotInfo(_outputOldIndex), GetSnapshotInfo(_outputNewIndex), _outputIsIncrease);
                    }
                    break;
            }
            return output;

        }

        /// <summary>
        /// 获取输出文件名
        /// </summary>
        /// <param name="isDifferent">是否是差异文件，否则是快照</param>
        /// <returns></returns>
        private string GetOutputFileName(bool isDifferent)
        {
            return isDifferent ? GetOutputDiffFileName() : GetOuputSnapshotFileName();
        }

        /// <summary>
        /// 获取输出差异文件名
        /// </summary>
        /// <returns></returns>
        private string GetOutputDiffFileName()
        {
            return string.Format("Different_{0}_{1}_{2}.csv", GetFormatTime(_outputNewIndex), _outputOldIndex, _outputNewIndex);
        }

        /// <summary>
        /// 获取输出快照文件名
        /// </summary>
        /// <returns></returns>
        private string GetOuputSnapshotFileName()
        {
            return string.Format("Snapshot_{0}_{1}.csv", GetFormatTime(_outputCurIndex), _outputCurIndex);
        }

        /// <summary>
        /// 获取格式化的快照时间
        /// </summary>
        /// <param name="index">快照索引</param>
        /// <returns></returns>
        private string GetFormatTime(int index)
        {
            return SnapshotUtil.FormatTime(GetSnapshotInfo(index).time);
        }


        #endregion

        #region 其他

        private void DrawOthers()
        {
            if (GUILayout.Button("清空所有数据"))
            {
                _snapshotMgr.ClearAll();
                _snapshotMgr.RefreshSnapshotType(_selectedType);
            }
        }

        #endregion
    }
}