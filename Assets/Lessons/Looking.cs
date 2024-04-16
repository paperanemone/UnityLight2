using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking : MonoBehaviour
{
    private readonly string MouseX = "Mouse X";
    private readonly string MouseY = "Mouse Y";

    [SerializeField] private float _speed;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _body;

    private void Update()
    {
        _camera.Rotate(_speed * -Input.GetAxis(MouseY) * Time.deltaTime * Vector3.right);
        _body.Rotate(_speed * Input.GetAxis(MouseX) * Time.deltaTime * Vector3.up);
    }
}
