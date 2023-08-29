using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private MovingACube cubePrefab;

    [SerializeField]
    private MoveDirection moveDirection;

    private int lvl = 0;
    private List<MovingACube> cubes_list = new List<MovingACube>();

    public GameObject Block;

    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab, Block.transform);

        //cubes_list.Add(cube);
        //cubes_list[cubes_list.Count - 1].SetLevel(4);
        lvl++;

        if (MovingACube.LastCube != null && MovingACube.LastCube.gameObject != GameObject.Find("Start"))
        {
            float x = moveDirection == MoveDirection.X ? transform.position.x : MovingACube.LastCube.transform.position.x;
            float z = moveDirection == MoveDirection.Z ? transform.position.z : MovingACube.LastCube.transform.position.z;

            cube.transform.position = new Vector3(
                x,
                MovingACube.LastCube.transform.position.y + cubePrefab.transform.localScale.y,
                z);
        }
        else
        {
            cube.transform.position = transform.position;
        }

        cube.MoveDirection = moveDirection;
        Debug.Log(MovingACube.CurrentCubeInst.transform.position.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }
}