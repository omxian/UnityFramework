using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Unity.Framework
{
    public static class Util
    {
        public static void CreateIfDirectoryNotExist(string savePath)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
        }

        public static void DeleteIfFileExist(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static double GetUTCTimestamp()
        {
            return (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static string GetCurrentDate()
        {
            return 
                DateTime.Now.Year.ToString() +
                DateTime.Now.Month.ToString() +
                DateTime.Now.Day.ToString() + "_" +
                DateTime.Now.Hour.ToString() +
                DateTime.Now.Minute.ToString();
        }

        /// <summary>
        /// Unity中的x方向为height/length, y方向为width
        /// 推导链接:http://jingyan.baidu.com/article/2c8c281dfbf3dd0009252a7b.html
        /// </summary>
        /// <param name="rectCenter">矩形中心点</param>
        /// <param name="rectAngle">矩形偏移的逆时针角度</param>
        /// <param name="width">y方向</param>
        /// <param name="height">x方向</param>
        /// <param name="checkPoint">检查点</param>
        /// <returnss>checkPoint是否在矩形中</returns>
        public static bool IsInRectangle(Vector3 rectCenter, float rectAngle, float width, float height, Vector3 checkPoint)
        {
            float radian = Mathf.PI * rectAngle / 180;
            float cos = Mathf.Cos(radian);
            float sin = Mathf.Sin(radian);
            float deltaX = checkPoint.x - rectCenter.x;
            float deltaZ = checkPoint.z - rectCenter.z;
            float newX = deltaX * cos - deltaZ * sin + rectCenter.x;
            float newZ = deltaX * sin + deltaZ * cos + rectCenter.z;
            float halfWidth = width * 0.5f;
            float halfHeight = height * 0.5f;

            if ((newX < rectCenter.x - halfHeight) || (newX > rectCenter.x + halfHeight) ||
                (newZ < rectCenter.z - halfWidth) || (newZ > rectCenter.z + halfWidth))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
