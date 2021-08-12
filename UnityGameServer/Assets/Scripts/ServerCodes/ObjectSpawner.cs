using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour
{

    public float speed = 0.1f;
    //public GameObject prefabs;
    float fillAmount = 0;
    public Vector3 spawnOfset;
    Image im;
    void Start()
    {
        im = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        //fillAmount += Time.deltaTime * speed;
        //im.fillAmount = fillAmount;
        //if (fillAmount >= 1)
        //{
        //    //var aa = Instantiate(prefabs, GameObject.FindGameObjectWithTag("objParent").transform);
        //    //aa.transform.position = transform.parent.parent.transform.position + spawnOfset;
        //    NetworkManager.instance.InstantiateNetworkObject(transform.position);
        //    fillAmount = 0f;
        //}
        if (Input.GetKey(KeyCode.A)) NetworkManager.instance.InstantiateNetworkObject(transform.position, 1);
        if (Input.GetKey(KeyCode.S)) NetworkManager.instance.InstantiateNetworkObject(transform.position, 2);
        if (Input.GetKey(KeyCode.D)) NetworkManager.instance.InstantiateNetworkObject(transform.position, 3);
        if (Input.GetKey(KeyCode.F)) NetworkManager.instance.InstantiateNetworkObject(transform.position, 4);
        if (Input.GetKey(KeyCode.G)) NetworkManager.instance.InstantiateNetworkObject(transform.position, 5); 
    }

    private void OnGUI()
    {
        
    }

}
