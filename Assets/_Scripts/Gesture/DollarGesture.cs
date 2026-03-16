using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Gestures/DollarGesture")]
public class DollarGesture : ScriptableObject
{
    public GameObject prefab;
    public string gestureName;
    public List<Vector2> points;
}