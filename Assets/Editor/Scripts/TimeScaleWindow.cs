using UnityEditor;
using UnityEngine;

namespace PajamaNinja.Editor.Scripts
{
    public class TimeScaleWindow : EditorWindow
    {
        float _timeScale;
        
        [MenuItem("Tools/TimeScaler")]
        private static void Init()
        {
            // Get existing open window or if none, make a new one:
            TimeScaleWindow window = (TimeScaleWindow)GetWindow(typeof(TimeScaleWindow));
            window.titleContent = new GUIContent("Time Scaler");
            window.Show();
            window._timeScale = Time.timeScale;
        }
        
        private void OnGUI()
        {
            _timeScale = EditorGUILayout.Slider("Slider", _timeScale, 0, 10);
            if (GUILayout.Button("Apply"))
                Time.timeScale = _timeScale;
            
        }
    }
}