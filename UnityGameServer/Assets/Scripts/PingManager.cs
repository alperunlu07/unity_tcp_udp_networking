using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingManager : MonoBehaviour
{
    private int counter = 0;
    private float lastTime = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if ((Time.time - lastTime) > 1f)
        {
            ServerSend.SendPing(counter);
            lastTime = Time.time;
            counter++;
        }
    }
}
