using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ForwardMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _startPoint;
   

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
}
