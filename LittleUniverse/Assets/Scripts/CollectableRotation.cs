using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableRotation : MonoBehaviour
{
    private float rotationSpeed = 5;

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed);
        transform.Rotate(Vector3.up * rotationSpeed);
        transform.Rotate(Vector3.right * rotationSpeed);
    }
}
