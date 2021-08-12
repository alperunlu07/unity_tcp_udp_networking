using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static float requestTime = 0;
    /// <summary>Sends a packet to the server via TCP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to the server via UDP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    /// <summary>Lets the server know that the welcome message was received.</summary>
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(ClientGameController.instance.UsrName);

            SendTCPData(_packet);
        }
    }

    /// <summary>Sends player input to the server.</summary>
    /// <param name="_inputs"></param>
    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);
        }
    }

    public static void PlayerShoot(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerShoot))
        {
            _packet.Write(_facing);

            SendTCPData(_packet);
        }
    }

    public static void PlayerThrowItem(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerThrowItem))
        {
            _packet.Write(_facing);

            SendTCPData(_packet);
        }
    }
    public static void SpawnNetworkObject(Vector3 _pos, int _type)
    {
        using (Packet _packet = new Packet((int)ClientPackets.spawnNetworkObject))
        {
            _packet.Write(_pos);
            _packet.Write(_type);
            SendTCPData(_packet);
        }
    }
    public static void RemoveNetworkObject(Vector3 _pos, int _type)
    {
        using (Packet _packet = new Packet((int)ClientPackets.spawnNetworkObject))
        {
            _packet.Write(_type);
            SendTCPData(_packet);
        }
    }

    public static void NetworkObjectPosition(NetworkObjectManager _obj)
    {
        using (Packet _packet = new Packet((int)ClientPackets.networkObjectPosition))
        {
            _packet.Write(_obj.id);
            _packet.Write(_obj.transform.position);
            
            SendTCPData(_packet);
        }
    }
    public static void NetworkObjectRotation(NetworkObjectManager _obj)
    {
        using (Packet _packet = new Packet((int)ClientPackets.networkObjectRotation))
        {
            _packet.Write(_obj.id);
            _packet.Write(_obj.transform.rotation);
            
            SendTCPData(_packet);
        }
    }
    public static void NetworkObjecScale(NetworkObjectManager _obj)
    {
        using (Packet _packet = new Packet((int)ClientPackets.networkObjectScale))
        {
            _packet.Write(_obj.id);
            _packet.Write(_obj.transform.localScale);
            
            SendTCPData(_packet);
        }
    }
    public static void NetworkObjectState(NetworkObjectManager _obj)
    {
        using (Packet _packet = new Packet((int)ClientPackets.networkObjectState))
        {
            _packet.Write(_obj.id);
            _packet.Write(_obj.state);            
            SendTCPData(_packet);
        }
    }
    public static void NetworkObjectFVal(NetworkObjectManager _obj)
    {
        using (Packet _packet = new Packet((int)ClientPackets.networkObjectFVal))
        {
            _packet.Write(_obj.id);
            _packet.Write(_obj.fVal);
            SendTCPData(_packet);
        }
    }
    public static void NetworkObjectSVal(NetworkObjectManager _obj)
    {
        using (Packet _packet = new Packet((int)ClientPackets.networkObjectSVal))
        {
            _packet.Write(_obj.id);
            _packet.Write(_obj.sVal);
            SendTCPData(_packet);
        }
    }

    public static void ObjectRequest(int _id)
    {
        if((Time.time-requestTime) > 0.1f)
        {
            using (Packet _packet = new Packet((int)ClientPackets.objectRequest))
            {
                _packet.Write(_id);
                SendTCPData(_packet);
            }
            requestTime = Time.time;
            //Debug.Log("not pos exist id: " + _id);
        }
    }
    public static void SendPing(int _id, bool _control)
    {
        using (Packet _packet = new Packet((int)ClientPackets.pingRequest))
        {
            _packet.Write(_id);
            _packet.Write(_control);
            SendTCPData(_packet);
        }
    }
    #endregion
}
