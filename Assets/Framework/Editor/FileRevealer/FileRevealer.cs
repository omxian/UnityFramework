﻿using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DeadMosquito.Revealer
{
    [InitializeOnLoad]
    public static class FileRevealer
    {
        static Texture2D _darkSkinTex;
        static Texture2D _lightSkinTex;

        const int Offset = 1;

        static FileRevealer()
        {
            LoadTextures();

            EditorApplication.projectWindowItemOnGUI += AddRevealerIcon;
        }

        static void LoadTextures()
        {
            _darkSkinTex = Resources.Load<Texture2D>("reveal-light");
            _lightSkinTex = Resources.Load<Texture2D>("reveal-dark");
        }

        static void AddRevealerIcon(string guid, Rect rect)
        {
            var isHover = rect.Contains(Event.current.mousePosition) && RevealerEditorSettings.ShowOnHover;
            var isSelected = IsSelected(guid) && RevealerEditorSettings.ShowOnSelected;

            bool isVisible = isHover || isSelected;

            if (!isVisible)
            {
                return;
            }

            float iconSize = EditorGUIUtility.singleLineHeight;
            var iconRect = new Rect(rect.width + rect.x - iconSize - RevealerEditorSettings.OffsetInProjectView, rect.y, 
                iconSize - Offset, iconSize - Offset);

            GUI.DrawTexture(iconRect, GetTex());

            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (GUI.Button(iconRect, GUIContent.none, GUIStyle.none))
            {
                EditorUtility.RevealInFinder(path);
            }

            EditorApplication.RepaintProjectWindow();
        }

        static Texture2D GetTex()
        {
            if (_darkSkinTex == null || _lightSkinTex == null)
            {
                LoadTextures();
            }

            return EditorGUIUtility.isProSkin ? _darkSkinTex : _lightSkinTex;;
        }

        static bool IsSelected(string guid)
        {
            return Selection.assetGUIDs.Any(guid.Contains);
        }
    }
}