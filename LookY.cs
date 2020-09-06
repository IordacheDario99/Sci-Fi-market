using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookY : MonoBehaviour
{
    [SerializeField] private float sensitivity = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float _lookY = - (Input.GetAxis("Mouse Y"));
        Vector3 newRotation = transform.localEulerAngles;
        newRotation.x += _lookY* sensitivity;
        transform.localEulerAngles = newRotation;

    }
}
