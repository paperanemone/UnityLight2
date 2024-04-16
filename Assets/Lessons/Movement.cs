using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private readonly string Horizontal = "Horizontal";
    private readonly string Vertical = "Vertical";

    [SerializeField] private float _speed;

    private void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis(Horizontal), 0f, Input.GetAxis(Vertical));

        transform.Translate(direction * _speed * Time.deltaTime);
    }
}
