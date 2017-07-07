using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// 将照片简单的转换为字符串形式
/// 使用方式：
/// 将脚本挂到任意对象上,配置参数
/// 点击右上角的选项，选择run
/// 注意导出目录是否正确
/// </summary>
public class PhotoToText : MonoBehaviour {
    
    private Dictionary<int, char> grayScaleDict = new Dictionary<int, char>() {
        { 0, '#' },
        { 1, '@' },
        { 2, '*' },
        { 3, 'G' },
        { 4, 'M' },
        { 5, 'm' },
        { 6, 'O' },
        { 7, 'o' },
        { 8, 'L' },
        { 9, 'l' },
        { 10, ' ' },
    };

    public Sprite sprite;
    public int pixelWidthStep = 10;
    public int pixelHeightStep = 10;

    [ContextMenu("Run")]
    void Start ()
    {
        int width = sprite.texture.width;
        int height = sprite.texture.height;

        StringBuilder result = new StringBuilder();

        string exportPath = Path.Combine(Application.dataPath , "Script/PhotoToText/Text");
        string exportFileName = Path.Combine(exportPath, sprite.name + ".txt");
        
        if (!Directory.Exists(exportPath))
        {
            Directory.CreateDirectory(exportPath);
        }
        if (File.Exists(exportFileName))
        {
            File.Delete(exportFileName);
        }

        for (int h = height; h >= 0; h = h - pixelHeightStep)
        {
            for (int w = 0; w < width; w = w + pixelWidthStep)
            {
                Color pixelColor = sprite.texture.GetPixel(w, h);
                result.Append(grayScaleDict[Mathf.RoundToInt(pixelColor.grayscale*10)]);
            }

            result.Append('\n');
            
            using (FileStream writer = File.Open(exportFileName, FileMode.Append))
            {
                byte[] fileData = Encoding.UTF8.GetBytes(result.ToString());
                writer.Write(fileData, 0, fileData.Length);
            }
            result.Length = 0;
        }
    }
}
