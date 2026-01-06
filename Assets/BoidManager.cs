using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public GameObject prefabBoid;
    public int numBoids = 50;
    public static BoidManager Instance;
    public List<GameObject> allBoids = new List<GameObject>();
    void Start()
    {
        Instance = this;
        for (int i = 0; i < numBoids; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(-5f, 5f),
                Random.Range(-5f, 5f),
                0
            );
            GameObject newBoid = Instantiate(prefabBoid, position, Quaternion.identity);
            allBoids.Add(newBoid);
        }
    }

    void Update()
    {
        
    }
}
