using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.transform.position = _position;
        }
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.transform.rotation = _rotation;
        }
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void PlayerHealth(Packet _packet)
    {
        int _id = _packet.ReadInt();
        float _health = _packet.ReadFloat();

        GameManager.players[_id].SetHealth(_health);
    }

    public static void PlayerRespawned(Packet _packet)
    {
        int _id = _packet.ReadInt();

        GameManager.players[_id].Respawn();
    }
       
    public static void SpawnNetworkObject(Packet _packet)
    {
        int _networkObjectId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _type= _packet.ReadInt();
        GameManager.instance.SpawnNetworkObject(_networkObjectId, _position, _type);
    }
    public static void RemoveNetworkObject(Packet _packet)
    {
        int _networkObjectId = _packet.ReadInt();        
        GameManager.instance.RemoveNetworkObject(_networkObjectId);
    }
    public static void NetworkObjectState(Packet _packet)
    {
        int _networkObjectId = _packet.ReadInt();
        int _state = _packet.ReadInt();
        GameManager.networkObjects[_networkObjectId].setState(_state);
    }
    public static void NetworkObjectFVal(Packet _packet)
    {
        int _networkObjectId = _packet.ReadInt();
        float _fVal = _packet.ReadFloat();
        GameManager.networkObjects[_networkObjectId].setFVal(_fVal);
    }
    public static void NetworkObjectSVal(Packet _packet)
    {
        int _networkObjectId = _packet.ReadInt();
        string _sVal = _packet.ReadString();
        GameManager.networkObjects[_networkObjectId].setSVal(_sVal);
    }
    public static void NetworkObjectPosition(Packet _packet)
    {
        int _networkObjectId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();        
        //GameManager.networkObjects[_networkObjectId].setPos(_position);
        if (GameManager.networkObjects.TryGetValue(_networkObjectId, out NetworkObjectManager _networkObject))
        {
            _networkObject.setPos(_position);
        }
        else
        {
            //Debug.Log("not pos exist id: " + _networkObjectId);
            ClientSend.ObjectRequest(_networkObjectId);
        }
    }
    public static void NetworkObjectRotation(Packet _packet)
    {
        int _networkObjectId = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();
        //GameManager.networkObjects[_networkObjectId].setRot(_rotation);
        if (GameManager.networkObjects.TryGetValue(_networkObjectId, out NetworkObjectManager _networkObject))
        {
            _networkObject.setRot(_rotation);
        }
        //else
        //{
        //    Debug.Log("not rot exist id: " + _networkObjectId);
        //    ClientSend.ObjectRequest(_networkObjectId);
        //}
    }
    public static void NetworkObjectScale(Packet _packet)
    {
        int _networkObjectId = _packet.ReadInt();
        Vector3 _scale = _packet.ReadVector3();
        if (GameManager.networkObjects.TryGetValue(_networkObjectId, out NetworkObjectManager _networkObject))
        {
            _networkObject.transform.localScale = _scale;
        }
    }

    public static void SendObjectRequest(Packet _packet)
    {

        int _networkObjectId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _type = _packet.ReadInt();
        if (GameManager.networkObjects.TryGetValue(_networkObjectId, out NetworkObjectManager _networkObject))
        {
            GameManager.instance.SpawnNetworkObject(_networkObjectId, _position, _type);
        }

        
    }

    public static void Ping(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool control = _packet.ReadBool();
        GameManager.CalculatePing(control, _id);
    }
}
