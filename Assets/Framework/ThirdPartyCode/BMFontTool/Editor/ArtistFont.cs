using UnityEngine;
using System.Collections;
using UnityEditor;

public class ArtistFont : MonoBehaviour
{	public static void BatchCreateArtistFont()
	{
		string dirName = "";
		string fntname = EditorUtils.SelectObjectPathInfo(ref dirName).Split('.')[0];
		string fntFileName = dirName + fntname + ".fnt";

        Font CustomFont = new Font();
        AssetDatabase.CreateAsset(CustomFont, dirName + fntname + ".fontsettings");

        TextAsset BMFontText = AssetDatabase.LoadAssetAtPath(fntFileName, typeof(TextAsset)) as TextAsset;

		BMFont mbFont = new BMFont();
		BMFontReader.Load(mbFont, BMFontText.name, BMFontText.bytes);  // 借用NGUI封装的读取类
		CharacterInfo[] characterInfo = new CharacterInfo[mbFont.glyphs.Count];

		for (int i = 0; i < mbFont.glyphs.Count; i++)
		{
			BMGlyph bmInfo = mbFont.glyphs[i];
			CharacterInfo info = new CharacterInfo();
			info.index = bmInfo.index;

            Rect rect = new Rect();
            rect.x = bmInfo.x / (float)mbFont.texWidth;
            rect.y = 1 - bmInfo.y / (float)mbFont.texHeight;
            rect.width = bmInfo.width / (float)mbFont.texWidth;
            rect.height = -1 * bmInfo.height / (float)mbFont.texHeight;

            info.uvBottomLeft = new Vector2(rect.xMin, rect.yMin);
            info.uvBottomRight = new Vector2(rect.xMax, rect.yMin);
            info.uvTopLeft = new Vector2(rect.xMin, rect.yMax);
            info.uvTopRight = new Vector2(rect.xMax,rect.yMax);
            
            rect = new Rect();
            rect.x = bmInfo.offsetX;
            rect.y = bmInfo.offsetY;
            rect.width = bmInfo.width;
            rect.height = bmInfo.height;
            
            info.minX = (int)rect.xMin;
            info.maxX = (int)rect.xMax;
            info.minY = (int)rect.yMax;
            info.maxY = (int)rect.yMin;

			info.advance = bmInfo.advance;
			characterInfo[i] = info;
		}
		CustomFont.characterInfo = characterInfo;

		string textureFilename = dirName + mbFont.spriteName + ".png";

		Shader shader = Shader.Find("Sprites/Default");
        Material mat = new Material(shader);
		Texture tex = AssetDatabase.LoadAssetAtPath(textureFilename, typeof(Texture)) as Texture;
		mat.SetTexture("_MainTex", tex);
		AssetDatabase.CreateAsset(mat, dirName + fntname + ".mat");

        AssetDatabase.SaveAssets();
        CustomFont.material = mat;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
