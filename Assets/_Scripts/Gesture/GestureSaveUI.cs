using System.Collections.Generic;
using UnityEngine;

public class GestureSaveUI : MonoBehaviour
{
    public GestureRecorder recorder;
    public string gestureName = "NewGesture";

    public void SaveGesture()
    {
#if UNITY_EDITOR
        GestureTemplateCreator.SaveGesture(gestureName, recorder.GetPoints());

        Debug.Log($"Gesture '{gestureName}' with {recorder.GetPoints().Count} points saved.");
#else
        Debug.LogWarning("Gesture saving via GestureSaveUI is editor-only.");
#endif
    }
}