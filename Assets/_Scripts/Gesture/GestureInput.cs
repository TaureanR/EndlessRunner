using System.Collections.Generic;
using UnityEngine;

public class GestureInput : MonoBehaviour
{
    public GestureSaveUI gestureSave;
    private List<Vector2> currentPoints = new List<Vector2>();
    private bool drawing = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentPoints.Clear();
            drawing = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            drawing = false;
            if (currentPoints.Count > 5)
            {
                float score;
                string match = DollarRecognizer.Instance.Recognize(currentPoints, out score);
                if (match != null)
                    Debug.Log($"Matched {match} with confidence {score:F2}");
                else
                    Debug.Log("No gesture match");
            }
        }

        if (drawing)
            currentPoints.Add(Input.mousePosition);
    }

    // Call this manually to record a new gesture for testing/training
    public void SaveGesture(string name)
    {
        DollarRecognizer.Instance.SaveGesture(name, currentPoints);
        Debug.Log($"Gesture '{name}' saved.");
    }
}