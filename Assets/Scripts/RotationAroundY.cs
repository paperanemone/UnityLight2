using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAroundY : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Update()
    {
        transform.Rotate(Vector3.up * _speed * Time.deltaTime);
    }
}
