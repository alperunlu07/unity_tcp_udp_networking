using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    public static Dictionary<int, NetworkObject> networkObjects = new Dictionary<int, NetworkObject>();
    private static int nextNetworkObjectId = 1;

    public int id;
    public int type;
    public int state;
    public float fVal;
    public string sVal;

    public NetworkObjectProperities networkObjectProperities;

    private int statePrev;
    private float fValPrev;
    private string sValPrev;

    private Vector3 posPrev;
    private Quaternion rotPrev;

    void Start()
    {
        id = nextNetworkObjectId;
        nextNetworkObjectId++;
        networkObjects.Add(id, this);
        ServerSend.SpawnNetworkObject(this);
        StartCoroutine(sendUDPData());
    }

    

    void Update()
    {
        if (fValPrev != fVal)
        {
            ServerSend.NetworkObjectFVal(this);
            fValPrev = fVal;
        }
        if (sValPrev != sVal)
        {
            ServerSend.NetworkObjectSVal(this);
            sValPrev = sVal;
        }
        if (statePrev != state)
        {
            ServerSend.NetworkObjectState(this);
            statePrev = state;
        }
    }
    private void FixedUpdate()
    {
   

    }


   
    IEnumerator sendUDPData()
    {
        while (true)
        {
            if (transform.hasChanged)
            {
                if (networkObjectProperities.position)
                {
                    if (Vector3.Distance(posPrev, transform.position) > 0.5f)
                    {
                        posPrev = transform.position;
                        ServerSend.NetworkObjectPosition(this);
                    }

                }
                if (networkObjectProperities.rotation)
                {
                    if (Vector3.Distance(rotPrev.eulerAngles, transform.rotation.eulerAngles) > 0.5f)
                    {
                        ServerSend.NetworkObjectRotation(this);
                        rotPrev = transform.rotation;
                    }
                }

                //if (networkObjectProperities.scale) ServerSend.NetworkObjectScale(this);
                transform.hasChanged = false;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                ServerSend.NetworkObjectPosition(this);
                ServerSend.NetworkObjectRotation(this);
            }
            //  yield return new WaitForEndOfFrame();

        }
    }

    internal void ChangeScale(Vector3 scale, int fromClient)
    {
        transform.localScale = scale;
        ServerSend.NetworkObjectScale(this, fromClient);
    }

    public void ChangePos(Vector3 _pos, int fromClient)
    {
        transform.position = _pos;
        ServerSend.NetworkObjectPosition(this,fromClient);
    }

    internal void ChangeState(int _state, int _fromClient)
    {
        state = _state;
        ServerSend.NetworkObjectState(this, _fromClient);
    }

    internal void ChangeFVal(float _fVal, int _fromClient)
    {
        fVal = _fVal;
        ServerSend.NetworkObjectFVal(this, _fromClient);
    }
    internal void ChangeSVal(string _sVal, int _fromClient)
    {
        sVal = _sVal;
        ServerSend.NetworkObjectSVal(this, _fromClient);
    }

    public void ChangeRot(Quaternion _rot, int fromClient)
    {
        transform.rotation = _rot;
        ServerSend.NetworkObjectRotation(this, fromClient);
    }

   

    public void sendState()
    {
        ServerSend.NetworkObjectState(this);
    }
    public void sendFVal()
    {
        ServerSend.NetworkObjectFVal(this);
    }
    public void sendSVal()
    {
        ServerSend.NetworkObjectSVal(this);
    }

    public void killObj()
    {
        ServerSend.RemoveNetworkObject(this);
        networkObjects.Remove(id);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("killZone"))
        {
            killObj();
        }
    }
    [System.Serializable]
    public class NetworkObjectProperities
    {
        public bool position;
        public bool rotation;
        public bool scale;
        public bool state;
        public bool fVal;
        public bool sVal;

    }
}
