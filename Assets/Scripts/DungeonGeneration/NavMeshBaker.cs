using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{

    private static NavMeshBaker instance;

    public static NavMeshBaker GetInstance()
    {
        if (NavMeshBaker.instance != null)
        {
            return NavMeshBaker.instance;
        }
        else
        {
            Debug.Log("You tried to call GetInstance() on NavMeshBaker, but NavMeshBaker has no instance.");
            return null;
        }

    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("======= WARNING ======= : You have created NavMeshBaker multiple times!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);


        }


    }








    [SerializeField]
    List<NavMeshSurface> navMeshSurfaces;

    // Start is called before the first frame update
    void Start()
    {
        navMeshSurfaces = new List<NavMeshSurface>();

        GetSurfaces();

        Build();
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

    void RemoveSurfaces()
    {
        navMeshSurfaces.Clear();
    }

    public void Build()
    {
        for (int i = 0; i < navMeshSurfaces.Count; i++)
        {
            navMeshSurfaces[i].BuildNavMesh();
        }
    }

    public void ResetBaker()
    {
        RemoveSurfaces();
        GetSurfaces();
        Build();
    }
}
