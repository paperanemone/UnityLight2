using UnityEngine;

public class Cube : MonoBehaviour
{
    public int SeparationChanceNumber { get; private set; } = 1;
    public float ExplosionForce { get; private set; } = 100f;
    public float ExplosionRadius { get; private set; } = 5f;

    public void IncreaseValues(GameObject parentCube)
    {
        IncreaseSeparationChanceNumber(parentCube);
        IncreaseExplosionForce(parentCube);
        IncreaseExplosionRadius(parentCube);
    }

    private void IncreaseSeparationChanceNumber(GameObject parentCube)
    {
        SeparationChanceNumber = parentCube.GetComponent<Cube>().SeparationChanceNumber * 2;
    }

    private void IncreaseExplosionForce(GameObject parentCube)
    {
        ExplosionForce = parentCube.GetComponent<Cube>().ExplosionForce * 2;
    }

    private void IncreaseExplosionRadius(GameObject parentCube)
    {
        ExplosionRadius = parentCube.GetComponent<Cube>().ExplosionRadius * 2;
    }
}
