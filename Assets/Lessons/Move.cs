using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private GetAxis _getAxis;

    private void OnEnable()
    {
      //  _getAxis.AxesChanged += MoveAxis;
    }

    private void OnDisable()
    {
       // _getAxis.AxesChanged -= MoveAxis;
    }

    public void MoveAxis()
    {
        float x = _getAxis.GetAxisX;
        float y = _getAxis.GetAxisY;


        transform.position += new Vector3(x * _speed * Time.deltaTime, y * _speed * Time.deltaTime, 0);
    }
}
