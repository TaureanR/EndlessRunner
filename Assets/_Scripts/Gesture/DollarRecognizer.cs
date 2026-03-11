using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DollarRecognizer : MonoBehaviour
{
    public static DollarRecognizer Instance;

    private List<DollarGesture> library = new List<DollarGesture>();
    private const int RESAMPLE_POINTS = 64;
    private const float SQUARE_SIZE = 250f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadAllGestures();
        }
        else Destroy(gameObject);
    }

    // ---------- Recognition ----------

    public string Recognize(List<Vector2> candidatePoints, out float score)
    {
        score = 0;

        if (candidatePoints == null || candidatePoints.Count < 2)
        {
            Debug.LogWarning("Not enough points to recognize gesture.");
            return null;
        }

        if (library.Count == 0)
        {
            Debug.LogWarning("No gestures in library!");
            return null;
        }

        var processedCandidate = Normalize(candidatePoints);
        if (processedCandidate == null || processedCandidate.Count < 2)
        {
            Debug.LogWarning("Processed candidate has too few points.");
            return null;
        }

        string bestName = null;
        float bestDistance = float.MaxValue;

        foreach (var g in library)
        {
            var processedTemplate = Normalize(g.points);
            if (processedTemplate == null || processedTemplate.Count != processedCandidate.Count)
                continue;

            float d = PathDistance(processedCandidate, processedTemplate);
            if (d < bestDistance)
            {
                bestDistance = d;
                bestName = string.IsNullOrEmpty(g.gestureName) ? g.name : g.gestureName;
            }
        }

        if (bestName == null)
        {
            score = 0f;
            return null;
        }

        score = 1f - (bestDistance / (0.5f * Mathf.Sqrt(SQUARE_SIZE * SQUARE_SIZE + SQUARE_SIZE * SQUARE_SIZE)));
        return bestName;
    }

    // ---------- Normalization Steps ----------

    private List<Vector2> Normalize(List<Vector2> points)
    {
        if (points == null || points.Count < 2)
            return null;

        List<Vector2> pts = Resample(points, RESAMPLE_POINTS);
        if (pts == null || pts.Count < 2)
            return null;

        pts = RotateToZero(pts);
        pts = ScaleToSquare(pts, SQUARE_SIZE);
        pts = TranslateToOrigin(pts);
        return pts;
    }

    private List<Vector2> Resample(List<Vector2> points, int n)
    {
        if (points == null || points.Count < 2 || n < 2)
            return null;

        float pathLength = PathLength(points);
        if (pathLength <= 0f)
            return null;

        float interval = pathLength / (n - 1);
        float D = 0f;

        List<Vector2> newPoints = new List<Vector2> { points[0] };

        for (int i = 1; i < points.Count; i++)
        {
            float d = Vector2.Distance(points[i - 1], points[i]);
            if (D + d >= interval)
            {
                float t = (interval - D) / d;
                Vector2 newPoint = Vector2.Lerp(points[i - 1], points[i], t);
                newPoints.Add(newPoint);
                points.Insert(i, newPoint);
                D = 0f;
            }
            else D += d;
        }

        if (newPoints.Count == n - 1)
            newPoints.Add(points[points.Count - 1]);

        return newPoints;
    }

    private float PathLength(List<Vector2> points)
    {
        float length = 0f;
        for (int i = 1; i < points.Count; i++)
            length += Vector2.Distance(points[i - 1], points[i]);
        return length;
    }

    private List<Vector2> RotateToZero(List<Vector2> points)
    {
        if (points == null || points.Count < 2)
            return points;

        Vector2 centroid = Centroid(points);
        float angle = Mathf.Atan2(points[0].y - centroid.y, points[0].x - centroid.x);
        return RotateBy(points, -angle, centroid);
    }

    private List<Vector2> RotateBy(List<Vector2> points, float angle, Vector2 center)
    {
        List<Vector2> newPoints = new List<Vector2>(points.Count);
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        foreach (var p in points)
        {
            float dx = p.x - center.x;
            float dy = p.y - center.y;
            newPoints.Add(new Vector2(dx * cos - dy * sin + center.x, dx * sin + dy * cos + center.y));
        }
        return newPoints;
    }

    private List<Vector2> ScaleToSquare(List<Vector2> points, float size)
    {
        Rect box = BoundingBox(points);
        List<Vector2> newPoints = new List<Vector2>(points.Count);
        foreach (var p in points)
        {
            float qx = p.x * (size / box.width);
            float qy = p.y * (size / box.height);
            newPoints.Add(new Vector2(qx, qy));
        }
        return newPoints;
    }

    private List<Vector2> TranslateToOrigin(List<Vector2> points)
    {
        Vector2 centroid = Centroid(points);
        List<Vector2> newPoints = new List<Vector2>(points.Count);
        foreach (var p in points)
            newPoints.Add(p - centroid);
        return newPoints;
    }

    private Rect BoundingBox(List<Vector2> points)
    {
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;
        foreach (var p in points)
        {
            if (p.x < minX) minX = p.x;
            if (p.x > maxX) maxX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.y > maxY) maxY = p.y;
        }
        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    private Vector2 Centroid(List<Vector2> points)
    {
        Vector2 c = Vector2.zero;
        foreach (var p in points) c += p;
        return c / points.Count;
    }

    private float PathDistance(List<Vector2> a, List<Vector2> b)
    {
        if (a == null || b == null || a.Count == 0 || b.Count == 0 || a.Count != b.Count)
            return float.MaxValue;

        float d = 0f;
        for (int i = 0; i < a.Count; i++)
            d += Vector2.Distance(a[i], b[i]);
        return d / a.Count;
    }

    // ---------- Library Management ----------

    public void SaveGesture(string name, List<Vector2> points)
    {
#if UNITY_EDITOR
        string folderPath = "Assets/Resources/Gestures";
        if (!AssetDatabase.IsValidFolder(folderPath))
            AssetDatabase.CreateFolder("Assets/Resources", "Gestures");

        string assetPath = folderPath + "/" + name + ".asset";

        DollarGesture gesture = AssetDatabase.LoadAssetAtPath<DollarGesture>(assetPath);
        if (gesture == null)
        {
            gesture = ScriptableObject.CreateInstance<DollarGesture>();
            AssetDatabase.CreateAsset(gesture, assetPath);
        }

        gesture.gestureName = name;
        gesture.points = new List<Vector2>(points);

        EditorUtility.SetDirty(gesture);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        if (!library.Contains(gesture))
            library.Add(gesture);

        Debug.Log($"Saved gesture asset: {assetPath}");
#else
        Debug.LogWarning("SaveGesture is editor-only; run in editor to create gesture assets.");
#endif
    }

    private void LoadAllGestures()
    {
        library.Clear();

#if UNITY_EDITOR
        string folderPath = "Assets/Resources/Gestures";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogWarning($"Gesture folder not found: {folderPath}");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:DollarGesture", new[] { folderPath });
        foreach (var guid in guids)
        {
            string objPath = AssetDatabase.GUIDToAssetPath(guid);
            DollarGesture g = AssetDatabase.LoadAssetAtPath<DollarGesture>(objPath);
            if (g != null)
                library.Add(g);
        }

        Debug.Log($"Loaded {library.Count} gestures from {folderPath}");
#else
        var gestures = Resources.LoadAll<DollarGesture>("Gestures");
        if (gestures == null || gestures.Length == 0)
        {
            Debug.LogWarning("No gestures found in Resources/Gestures.");
            return;
        }

        library.AddRange(gestures);
        Debug.Log($"Loaded {library.Count} gestures from Resources/Gestures");
#endif
    }
}