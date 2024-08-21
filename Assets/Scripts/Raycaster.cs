using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private Ray _ray;
    public RaycastHit Hit { get; private set; }

    private int _mouseButtonNumber = 0;

    public event Action RaycastHitted;

    private void Update()
    {
        if (Input.GetMouseButtonDown(_mouseButtonNumber))
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity, _layerMask.value))
            {
                Hit = hit;
                RaycastHitted?.Invoke();
            }
        }
    }
}
