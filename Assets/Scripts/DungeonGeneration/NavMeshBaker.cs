using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{

    [SerializeField]
    List<NavMeshSurface> navMeshSurfaces;

    // Start is called before the first frame update
    void Start()
    {
        navMeshSurfaces = new List<NavMeshSurface>();

        GetSurfaces();

        for (int i = 0; i < navMeshSurfaces.Count; i++)
        {
            navMeshSurfaces[i].BuildNavMesh();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetSurfaces()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("NavMeshSurface");
        for (int i = 0; i < array.Length; i++)
        {
            navMeshSurfaces.Add(array[i].GetComponent<NavMeshSurface>());
        }
    }
}
