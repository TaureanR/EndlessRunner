using System.Collections.Generic;
using UnityEngine;

public class GestureRecorder : MonoBehaviour
{
    public GestureSaveUI gestureSaveUI; // optional reference to editor save UI
    public string gestureName = "NewGesture";

    public LineRenderer lineRenderer;
    public float minPointDistance = 10f;
    
    private List<Vector2> points = new List<Vector2>();
    private bool drawing;
    private bool gestureFinished;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGesture();
        }

        if (Input.GetMouseButton(0))
        {
            RecordPoint();
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndGesture();
        }

        if (gestureFinished && Input.GetKeyDown(KeyCode.Space))
        {
            TrySaveGesture();
        }
    }

    void StartGesture()
    {
        drawing = true;
        points.Clear();

        lineRenderer.positionCount = 0;
    }

    void RecordPoint()
    {
        Vector2 pos = Input.mousePosition;

        if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], pos) > minPointDistance)
        {
            points.Add(pos);

            lineRenderer.positionCount = points.Count;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(pos.x, pos.y, Camera.main.nearClipPlane + 1f));

            lineRenderer.SetPosition(points.Count - 1, worldPos);
        }
    }

    void EndGesture()
    {
        drawing = false;
        gestureFinished = true;

        Debug.Log("Gesture drawn with " + points.Count + " points. Press Space to save.");
    }

    private void TrySaveGesture()
    {
        if (gestureSaveUI == null)
        {
            Debug.LogWarning("GestureSaveUI reference is missing. Assign in inspector.");
            return;
        }

        gestureSaveUI.recorder = this;
        gestureSaveUI.gestureName = gestureName;
        gestureSaveUI.SaveGesture();

        gestureFinished = false;
    }

    public List<Vector2> GetPoints()
    {
        return new List<Vector2>(points);
    }
}