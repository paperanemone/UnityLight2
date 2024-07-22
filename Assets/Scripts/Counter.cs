using System;
using System.Collections;
using UnityEngine;

public class Counter : MonoBehaviour
{
   private int _mouseButtonNumber = 0;
   private bool _isCoroutineWork = false;

   public event Action CountChanged;

   public int Count { get; private set; } = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(_mouseButtonNumber))
        {
            if (_isCoroutineWork == false)
            {
                _isCoroutineWork = true;
                StartCoroutine(NumberCounter());
            }
            else
            {
                _isCoroutineWork = false;
            }
        }
    }

    private IEnumerator NumberCounter() 
    {
        float delay = 0.5f;
        var wait = new WaitForSeconds(delay);

        while (_isCoroutineWork)
        {
            Count++;
            CountChanged?.Invoke();
            yield return wait;
        }
    }   
}
