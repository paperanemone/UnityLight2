using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private float _checkDistance;
    [SerializeField] private Transform _raycastPoint;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private BuildPreview _buildPreview;

    private RaycastHit _hitInfo;

    private Vector3 BuildPosition => _hitInfo.transform.position + _hitInfo.normal;

    private void Update()
    {
        if (_hitInfo.transform == null)
            return;

        if (_hitInfo.transform.GetComponent<Block>() == null)
            return;

        if (Input.GetMouseButtonDown(0))
            Build();

        if (Input.GetMouseButtonDown(1))
            if(_hitInfo.transform.TryGetComponent(out Block block))
                block.Destroy();
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(_raycastPoint.position, _raycastPoint.forward, out _hitInfo, _checkDistance))
        {
            if (_buildPreview.IsActive == false)
            {
                _buildPreview.Enable();
            }

            _buildPreview.SetPosition(BuildPosition);
        }
        else
            _buildPreview.Disable();
    }

    private void Build()
    {
        Vector3 position = BuildPosition;

        Instantiate(_blockPrefab, position, Quaternion.identity);
    }
}
