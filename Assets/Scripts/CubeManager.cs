using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private int _minRandomCubeCount = 2;
    private int _maxRandomCubeCount = 6;

    private float _explosionForce = 100f;
    private float _explosionRadius = 5f;

    public void ExplodeCube(GameObject cube)
    {
        if (Random.Range(1, cube.GetComponent<Cube>().SeparationChanceNumber + 1) == 1)
        {
            int randomCubeCount = Random.Range(_minRandomCubeCount, _maxRandomCubeCount + 1);

            for (int i = 0; i < randomCubeCount; i++)
            {
                CreateNewCube(cube).GetComponent<Rigidbody>().AddExplosionForce(_explosionForce, cube.transform.position, _explosionRadius);
            }
        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(cube.transform.position, _explosionRadius, _layerMask.value);
                
           foreach (Collider collider in colliders)
           {
               collider.attachedRigidbody.AddExplosionForce(cube.GetComponent<Cube>().ExplosionForce, cube.transform.position, cube.GetComponent<Cube>().ExplosionRadius);
           }
        }

        Destroy(cube);
    }

    private GameObject CreateNewCube(GameObject cube)
    {
        GameObject newCube = Instantiate(cube);

        newCube.GetComponent<Cube>().IncreaseValues(cube);

        newCube.transform.localScale = cube.transform.localScale / 2;

        newCube.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);

        return newCube;
    }
}
