using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolBox.Utils
{
    public static class ScreenUtils
    {
        public static bool WorldObjectIsWithinScreenRange(Transform objTrans, Camera cam)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(objTrans.position);

            Rect safeArea = Screen.safeArea;
            float xMax = safeArea.xMax;
            float xMin = safeArea.xMin;
            float yMax = safeArea.yMax;
            float yMin = safeArea.yMin;

            if (screenPos.x < xMin || screenPos.x > xMax || screenPos.y < yMin || screenPos.y > yMax)
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