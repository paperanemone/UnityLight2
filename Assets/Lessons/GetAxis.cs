using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GetAxis : MonoBehaviour
{
    const string Horizontal = nameof(Horizontal);
    const string Vertical = nameof(Vertical);

    [SerializeField] public UnityEvent AxesChanged;
    public float GetAxisX { get; private set; }
    public float GetAxisY { get; private set; }

    private void Update()
    {
        GetAxisX = Input.GetAxisRaw(Horizontal);
        GetAxisY = Input.GetAxisRaw(Vertical);

        AxesChanged?.Invoke();
    }
}
