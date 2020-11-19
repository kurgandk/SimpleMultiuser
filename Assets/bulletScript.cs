using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class bulletScript : NetworkBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * 6);

        GetComponent<Rigidbody2D>().velocity = transform.up * 6;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        var hit = collision.gameObject;
        if (hit.tag == "Player")
        {
            var PC = hit.GetComponent<PlayerController>();
            Debug.Log("deal dmg");
            PC.takeDMG();
        }
    }
}
