using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, ItemSpawner> itemSpawners = new Dictionary<int, ItemSpawner>();
    public static Dictionary<int, ProjectileManager> projectiles = new Dictionary<int, ProjectileManager>();
    public static Dictionary<int, EnemyManager> enemies = new Dictionary<int, EnemyManager>();
    public static Dictionary<int, SmileManager> smiles = new Dictionary<int, SmileManager>();
    public static Dictionary<int, NetworkObjectManager> networkObjects = new Dictionary<int, NetworkObjectManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject itemSpawnerPrefab;
    public GameObject projectilePrefab;
    public GameObject enemyPrefab;
    public GameObject smilePrefab;
    public GameObject[] networkObjectsPrefabs;
    public static GameObject guiText;

    private static int second;
    private static int milisecond;

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

    public static void CalculatePing(int mili, int id)
    {
        var _mili = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
        //Debug.Log("id " + id + " time: " + ping + " nowTime" + _mili);

        ping = (_mili - mili) * 2;

        //Debug.Log("Ping:" + (_mili - ping) * 2 + " ms");
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

    public void CreateItemSpawner(int _spawnerId, Vector3 _position, bool _hasItem)
    {
        GameObject _spawner = Instantiate(itemSpawnerPrefab, _position, itemSpawnerPrefab.transform.rotation);
        _spawner.GetComponent<ItemSpawner>().Initialize(_spawnerId, _hasItem);
        itemSpawners.Add(_spawnerId, _spawner.GetComponent<ItemSpawner>());
    }

    public void SpawnProjectile(int _id, Vector3 _position)
    {
        GameObject _projectile = Instantiate(projectilePrefab, _position, Quaternion.identity);
        _projectile.GetComponent<ProjectileManager>().Initialize(_id);
        projectiles.Add(_id, _projectile.GetComponent<ProjectileManager>());
    }

    public void SpawnEnemy(int _id, Vector3 _position)
    {
        GameObject _enemy = Instantiate(enemyPrefab, _position, Quaternion.identity);
        _enemy.GetComponent<EnemyManager>().Initialize(_id);
        enemies.Add(_id, _enemy.GetComponent<EnemyManager>());
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
