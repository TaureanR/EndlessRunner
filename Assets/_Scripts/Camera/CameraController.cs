using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController playerController;
    public Transform target;
    private Vector3 offset;
    private float horizOffset;

    // Camera shake variables
    private bool isShaking = false;
    private float shakeDuration = 0.2f;
    private float shakeMagnitude = 0.1f;
    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        SetOffset();
        originalPosition.y = transform.position.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        HandleMove();
    }

    void HandleMove()
    {
        Vector3 newPosition = new Vector3(horizOffset + target.position.x, originalPosition.y, offset.z + target.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, 15 * Time.deltaTime);

        if (isShaking)
        {
            transform.position += Random.insideUnitSphere * shakeMagnitude;
        }
    }

    void SetOffset()
    {
        // Sets the offset for the cameras x and z position
        horizOffset = transform.position.x - target.position.x;
        offset = transform.position - target.position;
    }

    public void ScreenShake()
    {
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        isShaking = true;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        originalPosition.y = 2.7f;

        isShaking = false;
    }
}
