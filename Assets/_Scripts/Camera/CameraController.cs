using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController playerController;
    public Transform target;
    private Vector3 offset;
    private float horizOffset;

    // Start is called before the first frame update
    void Start()
    {
        SetOffset();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        HandleMove();
    }

    void HandleMove()
    {
        Vector3 newPosition = new Vector3(horizOffset + target.position.x, transform.position.y, offset.z + target.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, 15 * Time.deltaTime);
    }

    void SetOffset()
    {
        // Sets the offset for the cameras x and z position
        horizOffset = transform.position.x - target.position.x;
        offset = transform.position - target.position;
    }

}
