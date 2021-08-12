using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();

        Server.clients[_fromClient].player.SetInput(_inputs, _rotation);
    }

    public static void PlayerShoot(int _fromClient, Packet _packet)
    {
        Vector3 _shootDirection = _packet.ReadVector3();

        //Server.clients[_fromClient].player.Shoot(_shootDirection);
    }

    public static void PlayerThrowItem(int _fromClient, Packet _packet)
    {
        Vector3 _throwDirection = _packet.ReadVector3();

        //Server.clients[_fromClient].player.ThrowItem(_throwDirection);
    }
    public static void SpawnNetworkObject(int _fromClient, Packet _packet)
    {
        Vector3 spawnPos = _packet.ReadVector3();
        int objType = _packet.ReadInt();
        NetworkManager.instance.InstantiateNetworkObject(spawnPos, objType);
    }
    public static void RemoveNetworkObject(int _fromClient, Packet _packet)
    {
        int objId = _packet.ReadInt();
         
    }
    public static void NetworkObjectPosition(int _fromClient, Packet _packet)
    {
        int id = _packet.ReadInt();
        Vector3 pos = _packet.ReadVector3();
        NetworkObject.networkObjects[id].ChangePos(pos, _fromClient);

    }
    public static void NetworkObjectRotation(int _fromClient, Packet _packet)
    {
        int id = _packet.ReadInt();
        Quaternion rot = _packet.ReadQuaternion();        
        NetworkObject.networkObjects[id].ChangeRot(rot, _fromClient);
    }

    internal static void NetworkObjectScale(int _fromClient, Packet _packet)
    {
        int id = _packet.ReadInt();
        Vector3 scale = _packet.ReadVector3();
        NetworkObject.networkObjects[id].ChangeScale(scale, _fromClient);
    }

    internal static void NetworkObjectState(int _fromClient, Packet _packet)
    {
        int id = _packet.ReadInt();
        int state = _packet.ReadInt();
        NetworkObject.networkObjects[id].ChangeState(state, _fromClient);
    }

    internal static void NetworkObjectFVal(int _fromClient, Packet _packet)
    {
        int id = _packet.ReadInt();
        float fVal = _packet.ReadFloat();
        NetworkObject.networkObjects[id].ChangeFVal(fVal, _fromClient);
    }

    internal static void NetworkObjectSVal(int _fromClient, Packet _packet)
    {
        int id = _packet.ReadInt();
        string sVal = _packet.ReadString();
        NetworkObject.networkObjects[id].ChangeSVal(sVal, _fromClient);
    }

    internal static void ObjectRequest(int _fromClient, Packet _packet)
    {
        int id = _packet.ReadInt();
        //ServerSend.SendObjectRequest(id, _fromClient);
    }
}
