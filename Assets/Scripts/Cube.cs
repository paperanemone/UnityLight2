using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int SeparationChanceNumber { get; private set; } = 1;

    public void IncreaseSeparationChanceNumber(GameObject parentCube)
    {
        SeparationChanceNumber = parentCube.GetComponent<Cube>().SeparationChanceNumber * 2;
    }
}
