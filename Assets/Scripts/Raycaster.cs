using UnityEngine;

public class Raycaster : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private CubeManager _cubeManager;

    private Camera _camera;

    private Ray _ray;

    private int _mouseButtonNumber = 0;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(_mouseButtonNumber))
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity, _layerMask.value))
            {
                _cubeManager.ExplodeCube(hit.collider.gameObject);
            }
        }
    }
}
