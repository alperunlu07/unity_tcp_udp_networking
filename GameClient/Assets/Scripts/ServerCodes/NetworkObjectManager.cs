using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObjectManager : MonoBehaviour
{

    public int id;
    public int type;
    public int state;
    public float fVal;
    public string sVal;

    private int statePrev;
    private float fValPrev;
    private string sValPrev;

    public bool moveWithClient = false;
    private bool moveWithClientPrev = false;
    float moveTime = 0f;

    public void Initialize(int _id,int _type)
    {
        id = _id;
        type = _type;
    }

    public void setState(int _state)
    {
        state = _state;
    }
    public void setFVal(float _fVal)
    {
        fVal = _fVal;
    }
    public void setSVal(string _sVal)
    {
        sVal = _sVal;
    }
    public void setPos(Vector3 pos)
    {
        if(!moveWithClient && moveTime == 0f)
        {
            transform.position = pos;
        }
            
    }
    public void setRot(Quaternion rot)
    {
        if (!moveWithClient && moveTime == 0f)
            transform.rotation = rot; 
    }


    void Start()
    {

    }


    void Update()
    {
        if(!moveWithClient && moveWithClientPrev)
        {
            ClientSend.NetworkObjectPosition(this);
            ClientSend.NetworkObjectRotation(this);
            moveWithClientPrev = moveWithClient;
            //StartCoroutine(waitUntil());
        }
        else if(moveWithClient != moveWithClientPrev)
        {
            moveWithClientPrev = moveWithClient;
            moveTime = Time.time;
        }
        if(!moveWithClient && moveTime !=0 && (Time.time- moveTime)> 0.1f)
        {
            moveTime = 0f;
        }

        if (fValPrev != fVal)
        {
            ClientSend.NetworkObjectFVal(this);
            fValPrev = fVal;
        }
        if (sValPrev != sVal)
        {
            ClientSend.NetworkObjectSVal(this);
            sValPrev = sVal;
        }
        if (statePrev != state)
        {
            ClientSend.NetworkObjectState(this);
            statePrev = state;
        }
    }
    //IEnumerator waitUntil()
    //{
    //    yield return new WaitForSeconds(1f);
    //    moveWait = false;
    //}
    public void killItself()
    {
        Destroy(gameObject);
    }
}
