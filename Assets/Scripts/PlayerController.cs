using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public GameObject cannonballPrefab;

    Player player;
    Rigidbody rb;

    WaterToy waterToy;


    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
        rb = GetComponent<Rigidbody>();
        waterToy = GetComponent<WaterToy>();
    }


    private void Update()
    {

        

        float ySpeed = rb.velocity.y;

        if (waterToy.isInWater)
            rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * 4f, 0.1f);

        rb.velocity = new Vector3(rb.velocity.x, ySpeed, rb.velocity.z);

        rb.MoveRotation(Quaternion.Euler(0, rb.rotation.eulerAngles.y + (100 * player.GetAxis(RewiredConsts.Action.Turn)) * Time.deltaTime, 0));

        if (player.GetAxis(RewiredConsts.Action.Shoot) != 0 && player.GetAxisPrev(RewiredConsts.Action.Shoot) == 0)
        {
            Vector3 cbDirection = transform.right;
            if (player.GetAxis(RewiredConsts.Action.Shoot) < 0) cbDirection = -transform.right;
            Cannonball cb = Instantiate(cannonballPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity).GetComponent<Cannonball>();
            cb.creator = this.gameObject;
            cb.transform.LookAt(cb.transform.position + cbDirection);
            
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        rb.AddForce(Vector3.Scale(collision.contacts[0].normal, new Vector3(1, 0, 1) * 50f), ForceMode.VelocityChange);
        Debug.Log(Vector3.Scale(collision.contacts[0].normal, new Vector3(1, 0, 1) * 50f));
    }

}
