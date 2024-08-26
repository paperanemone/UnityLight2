using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Raycaster _raycaster;

    private int _separationChanceNumber = 1;
    private int _minRandomCubeCount = 2;
    private int _maxRandomCubeCount = 6;

    private float _explosionForce = 100f;
    private float _explosionRadius = 5f;

    private void OnEnable()
    {
        _raycaster.RaycastHitted += ExplodeCube;
    }

    private void OnDisable()
    {
        _raycaster.RaycastHitted -= ExplodeCube;
    }

    private void ExplodeCube()
    {
        if (gameObject == _raycaster.Hit.collider.gameObject)
        {
            if (Random.Range(1, _separationChanceNumber + 1) == 1)
            {
                int randomCubeCount = Random.Range(_minRandomCubeCount, _maxRandomCubeCount + 1);

                for (int i = 0; i < randomCubeCount; i++)
                {
                    CreateNewCube().GetComponent<Rigidbody>().AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
                }
            }

            Destroy(gameObject);
        }
    }

    private GameObject CreateNewCube()
    {
        GameObject newCube = Instantiate(_cubePrefab);

        newCube.GetComponent<CubeManager>()._separationChanceNumber = _separationChanceNumber * 2;

        newCube.transform.localScale = transform.localScale / 2;

        newCube.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);

        return newCube;
    }
}
