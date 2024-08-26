using System;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private Ray _ray;

    private int _mouseButtonNumber = 0;

    public event Action RaycastHitted;

    public RaycastHit Hit { get; private set; }

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
