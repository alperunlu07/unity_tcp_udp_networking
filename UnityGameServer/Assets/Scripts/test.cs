using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Rigidbody2D rb;
    float jumpSpeed = 300f;
    public bool isGround = false;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(jumpCO());
    }


    void Update()
    { 

    }
    IEnumerator jumpCO()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(isGround)
                rb.AddForce(Vector2.up * jumpSpeed);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGround = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGround = false;
    }

}
