using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
   private int _count = 0;
    private bool _isCoroutineWork = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isCoroutineWork == false)
            {
                _isCoroutineWork = true;
                StartCoroutine(NumberCounter());
            }
            else
            {
                _isCoroutineWork = false;
                StopCoroutine(NumberCounter());
            }
        }
    }

    private IEnumerator NumberCounter() 
    {
        float delay = 0.5f;

        while (_isCoroutineWork)
        {
            _count++;
            Debug.Log(_count);
            yield return new WaitForSeconds(delay);
        }
    }   
}
