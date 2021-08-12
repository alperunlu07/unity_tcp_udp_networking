using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smile : MonoBehaviour
{
    public static Dictionary<int, Smile> smiles = new Dictionary<int, Smile>();
    private static int nextSmileId = 1;

    public int id;
    public int state = 0;
    void Start()
    {
        id = nextSmileId;
        nextSmileId++;
        smiles.Add(id, this);

        //ServerSend.SpawnSmile(this);
    }
     
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        //ServerSend.SmilePosition(this);
        //ServerSend.SmileRotation(this);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("killZone"))
        {
            state = -1;
            //ServerSend.SmileState(this);
            Destroy(gameObject);
        }
    }


}
