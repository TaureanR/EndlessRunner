using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get a reference to the target object
        PlayerController playerController = (PlayerController)target;

        // Display a slider for the "movementSpeed" property
        playerController.ForwardSpeed = EditorGUILayout.Slider("Movement Speed", playerController.ForwardSpeed, 0, 80);

        // Display a slider for the "jumpForce" property
        playerController.JumpForce = EditorGUILayout.Slider("Jump Force", playerController.JumpForce, 0, 10);

        // Display a slider for the "gravity" property
        playerController.Gravity = EditorGUILayout.Slider("Gravity", playerController.Gravity, -50, 20);
    }
}