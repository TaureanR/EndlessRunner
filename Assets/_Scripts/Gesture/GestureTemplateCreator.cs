using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class GestureTemplateCreator
{
    public static void SaveGesture(string name, List<Vector2> points)
    {
#if UNITY_EDITOR
        DollarGesture gesture = ScriptableObject.CreateInstance<DollarGesture>();

        gesture.gestureName = name;
        gesture.points = new List<Vector2>(points);

        string folderPath = "Assets/Resources/Gestures";
        if (!AssetDatabase.IsValidFolder(folderPath))
            AssetDatabase.CreateFolder("Assets/Resources", "Gestures");

        string path = folderPath + "/" + name + ".asset";

        AssetDatabase.CreateAsset(gesture, path);
        AssetDatabase.SaveAssets();

        Debug.Log("Gesture template saved: " + path);
#else
        Debug.LogWarning("GestureTemplateCreator.SaveGesture is only available in the editor.");
#endif
    }
}