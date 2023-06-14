using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationDirection = default;
    [SerializeField] private float _rotationSpeed = 10.0f;
  
    void Update()
    {
        transform.Rotate(_rotationSpeed * _rotationDirection * Time.deltaTime);
    }
}
