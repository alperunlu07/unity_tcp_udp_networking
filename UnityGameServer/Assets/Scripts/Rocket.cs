using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public static Dictionary<int, Rocket> rockets = new Dictionary<int, Rocket>();
    private static int nextRocketId = 1;

    public int id;
    public int type;
    void Start()
    {
        id = nextRocketId;
        nextRocketId++;
        rockets.Add(id, this);

        //ServerSend.SpawnRocket(this);
    }


    void Update()
    {
        
    }
}
