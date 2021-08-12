using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientGameController : MonoBehaviour
{
    public static ClientGameController instance;

    public string UsrName = "";
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        Client.instance.ConnectToServer();
    }

     
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) ClientSend.SpawnNetworkObject(transform.position, 1);
        if (Input.GetKeyDown(KeyCode.S)) ClientSend.SpawnNetworkObject(transform.position, 2);
        if (Input.GetKeyDown(KeyCode.D)) ClientSend.SpawnNetworkObject(transform.position, 3);
        if (Input.GetKeyDown(KeyCode.F)) ClientSend.SpawnNetworkObject(transform.position, 4);
        if (Input.GetKeyDown(KeyCode.G)) ClientSend.SpawnNetworkObject(transform.position, 5);
    }
}
