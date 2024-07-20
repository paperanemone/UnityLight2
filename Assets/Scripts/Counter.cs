using System.Collections;
using UnityEngine;

public class Counter : MonoBehaviour
{
   private int _count = 0;
   private int _mouseButtonNumber = 0;
   private bool _isCoroutineWork = false;

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
            _count++;
            Debug.Log(_count);
            yield return wait;
        }
    }   
}
