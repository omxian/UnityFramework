using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Unity.Framework
{
    public static class ButtonExtentions
    {
        public static void AddClick(this Button button, Action click, string sfx = "")
        {
            button.onClick.AddListener(() => {
                // Play Click Sound 
                if (null != click)
                {
                    click();
                }
            });
        }

        public static void RemoveClick(this Button button)
        {
            button.onClick.RemoveAllListeners();
        }

        public static void SetActive(this Button button,bool isActive)
        {
            button.gameObject.SetActive(isActive);
        }
    }
}
