using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();    
    public static Dictionary<int, NetworkObjectManager> networkObjects = new Dictionary<int, NetworkObjectManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;   
    public GameObject[] networkObjectsPrefabs;
    public static GameObject guiText;

    private static int startTime;
    private int counter = 0;
    private float lastTime = 0f;
    private bool control;

    private static int ping;

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
    private void FixedUpdate()
    {
        if ((Time.time - lastTime) > 1f)
        {
            startTime = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
            counter++;
            ClientSend.SendPing(counter, control);
            lastTime = Time.time;
        }
    }
    public static void CalculatePing(bool _control, int id)
    {
        if (_control)
        {
            int currentTime = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
            ping = currentTime - startTime;
        }
    }

    public void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h / 10;
        style.normal.textColor = new Color(1.0f, 0.0f, 0.5f, 1.0f);


        string text = string.Format("{0:0.0} ms", ping);
        GUI.Label(rect, text, style);
    }

    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>
    /// <param name="_position">The player's starting position.</param>
    /// <param name="_rotation">The player's starting rotation.</param>
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }
        Debug.Log(_id);
        Debug.Log(_username);
        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    
    public void SpawnNetworkObject(int _id, Vector3 _position, int _type)
    {
        if (GameManager.networkObjects.TryGetValue(_id, out NetworkObjectManager __networkObject))
        {
            return;
        }
        GameObject _networkObject = Instantiate(networkObjectsPrefabs[_type], _position, Quaternion.identity);
        _networkObject.GetComponent<NetworkObjectManager>().Initialize(_id, _type);
        networkObjects.Add(_id, _networkObject.GetComponent<NetworkObjectManager>());
    }

    public void RemoveNetworkObject(int _id)
    {
        if (GameManager.networkObjects.TryGetValue(_id, out NetworkObjectManager _networkObject))
        {
            _networkObject.killItself();
            networkObjects.Remove(_id);
        }        
    }

}
