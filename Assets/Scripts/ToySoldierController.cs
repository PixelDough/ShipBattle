using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ToySoldierController : MonoBehaviour
{

    Player player;
    Animator animator;
    Rigidbody rb;


    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {

        Vector3 input = new Vector3(player.GetAxis(RewiredConsts.Action.WalkHorizontal), 0, player.GetAxis(RewiredConsts.Action.WalkVertical));
        Vector3 groundVelocity = Vector3.Lerp(rb.velocity, input * 10f, 10f * Time.deltaTime);

        if (!Physics.Raycast(transform.position + (Vector3.right * input.x), Vector3.down, 0.1f))
        {
            groundVelocity.x = 0;
        }
        if (!Physics.Raycast(transform.position + (Vector3.forward * input.z), Vector3.down, 0.1f))
        {
            groundVelocity.z = 0;
        }

        rb.velocity = new Vector3(groundVelocity.x, rb.velocity.y, groundVelocity.z);

        if (input != Vector3.zero)
        {
            animator.Play("walk");
            Quaternion targetRotation = Quaternion.LookRotation(input, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            
        }
        else
        {
            animator.Play("idle");
        }

    }

}
