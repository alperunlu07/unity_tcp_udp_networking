using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    /// <summary>Sends a packet to a client via TCP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        try
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
       
    }

    /// <summary>Sends a packet to a client via UDP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    /// <summary>Sends a packet to all clients via TCP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via TCP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    /// <summary>Sends a packet to all clients via UDP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via UDP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

   

    #region Packets
    /// <summary>Sends a welcome message to the given client.</summary>
    /// <param name="_toClient">The client to send the packet to.</param>
    /// <param name="_msg">The message to send.</param>
    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>Tells a client to spawn a player.</summary>
    /// <param name="_toClient">The client that should spawn the player.</param>
    /// <param name="_player">The player to spawn.</param>
    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>Sends a player's updated position to all clients.</summary>
    /// <param name="_player">The player whose position to update.</param>
    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>Sends a player's updated rotation to all clients except to himself (to avoid overwriting the local player's rotation).</summary>
    /// <param name="_player">The player whose rotation to update.</param>
    public static void PlayerRotation(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.rotation);

            SendUDPDataToAll(_player.id, _packet);
        }
    }

    public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_packet);
        }
    }
     
    public static void SpawnNetworkObject(NetworkObject _networkObject)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnNetworkObject))
        {
            SendTCPDataToAll(SpawnNetworkObject_Data(_networkObject, _packet));
        }
    }
    public static void SpawnNetworkObject(int _toClient, NetworkObject _networkObject)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnNetworkObject))
        {
            SendTCPData(_toClient, SpawnNetworkObject_Data(_networkObject, _packet));
        }
    }
    public static void RemoveNetworkObject(NetworkObject _networkObject)
    {
        using (Packet _packet = new Packet((int)ServerPackets.removeNetworkObject))
        {
            _packet.Write(_networkObject.id);
            SendTCPDataToAll(_packet);
        }
    }
    private static Packet SpawnNetworkObject_Data(NetworkObject _networkObject, Packet _packet)
    {
        try
        {
            if (_networkObject.transform != null)
            {
                _packet.Write(_networkObject.id);
                _packet.Write(_networkObject.transform.position);
                _packet.Write(_networkObject.type);
                return _packet;
            }            
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        return null;

    }
    public static void NetworkObjectState(NetworkObject _networkObject)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkObjectState))
        {
            _packet.Write(_networkObject.id);
            _packet.Write(_networkObject.state);
            SendTCPDataToAll(_packet);
        }
    }
    public static void NetworkObjectState(NetworkObject _networkObject, int _fromClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkObjectState))
        {
            _packet.Write(_networkObject.id);
            _packet.Write(_networkObject.state);
            SendTCPDataToAll(_fromClient, _packet);
        }
    }
    public static void NetworkObjectFVal(NetworkObject _networkObject)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkObjectFVal))
        {
            _packet.Write(_networkObject.id);
            _packet.Write(_networkObject.fVal);
            SendTCPDataToAll(_packet);
        }
    }
    public static void NetworkObjectFVal(NetworkObject _networkObject, int _fromClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkObjectFVal))
        {
            _packet.Write(_networkObject.id);
            _packet.Write(_networkObject.fVal);
            SendTCPDataToAll(_fromClient, _packet);
        }
    }
    public static void NetworkObjectSVal(NetworkObject _networkObject)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkObjectSVal))
        {
            _packet.Write(_networkObject.id);
            _packet.Write(_networkObject.sVal);
            SendTCPDataToAll(_packet);
        }
    }
    public static void NetworkObjectSVal(NetworkObject _networkObject, int _fromClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkObjectSVal))
        {
            _packet.Write(_networkObject.id);
            _packet.Write(_networkObject.sVal);
            SendTCPDataToAll(_fromClient,_packet);
        }
    }
    public static void NetworkObjectPosition(NetworkObject _networkObject)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkObjectPosition))
        {
            _packet.Write(_networkObject.id);
            _packet.Write(_networkObject.transform.position);

            //SendUDPDataToAll(_packet);
            SendTCPDataToAll(_packet);
        }
    }
    /// <summary>Sends a position to all clients except one via UDP.</summary>
    /// <param name="_exceptClient">The client to NOT send the position data to.</param>
    /// <param name="_packet">The packet to send.</param>
    public static void NetworkObjectPosition(NetworkObject _networkObject, int _fromClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkObjectPosition))
        {
            _packet.Write(_networkObject.id);
            _packet.Write(_networkObject.transform.position);

            SendUDPDataToAll(_fromClient, _packet);
        }
    }
    public static void NetworkObjectRotation(NetworkObject _networkObject)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkObjectRotation))
        {
            _packet.Write(_networkObject.id);
            _packet.Write(_networkObject.transform.rotation);

            SendUDPDataToAll(_packet);
        }
    }
    public static void NetworkObjectRotation(NetworkObject _networkObject, int _fromClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkObjectRotation))
        {
            _packet.Write(_networkObject.id);
            _packet.Write(_networkObject.transform.rotation);
            SendUDPDataToAll(_fromClient, _packet);
        }
    }
    public static void NetworkObjectScale(NetworkObject _networkObject)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkObjectScale))
        {
            _packet.Write(_networkObject.id);
            _packet.Write(_networkObject.transform.localScale);
            SendUDPDataToAll(_packet);
        }
    }
    public static void NetworkObjectScale(NetworkObject _networkObject, int _fromClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkObjectScale))
        {
            _packet.Write(_networkObject.id);
            _packet.Write(_networkObject.transform.localScale);
            SendUDPDataToAll(_fromClient, _packet);
        }
    }

    public static void SendObjectRequest(int _id, int _fromClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.sendObjectRequest))
        {
            //if (NetworkObject.networkObjects.TryGetValue(_id, out NetworkObject _networkObject))
            //{
            //    //var _networkObject = NetworkObject.networkObjects[_id];
            //    _packet.Write(_networkObject.id);
            //    _packet.Write(_networkObject.transform.position);
            //    _packet.Write(_networkObject.type);
            //    SendTCPData(_fromClient, _packet);
            //}
            var _networkObject = NetworkObject.networkObjects[_id];
            try
            {
                if (_networkObject.transform != null)
                {
                    _packet.Write(_networkObject.id);
                    _packet.Write(_networkObject.transform.position);
                    _packet.Write(_networkObject.type);
                    SendTCPData(_fromClient, _packet);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            
        }
    }

    public static void SendPing(int id)
    {
        using (Packet _packet = new Packet((int)ServerPackets.ping))
        {
            _packet.Write(id);
            int nowTime = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
            _packet.Write(nowTime);
            SendUDPDataToAll(_packet);
        }
    }
    #endregion
}
