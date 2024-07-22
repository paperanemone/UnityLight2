using UnityEngine;

public class CountRenderer : MonoBehaviour
{
    [SerializeField] private Counter _counter;

    private void OnEnable()
    {
        _counter.CountChanged += ShowCount;
    }

    private void OnDisable()
    {
        _counter.CountChanged -= ShowCount;
    }

    private void ShowCount()
    {
        Debug.Log(_counter.Count);
    }
}
