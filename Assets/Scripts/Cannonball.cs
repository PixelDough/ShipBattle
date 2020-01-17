using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cannonball : MonoBehaviour
{

    public GameObject creator;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(CannonLife());
    }


    private IEnumerator CannonLife()
    {

        rb.velocity = transform.forward * 20f;
        
        yield return new WaitForSeconds(0.5f);

        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;

        while (true)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0f, rb.velocity.y, 0f), 0.02f);
            yield return null;
        }

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.gameObject == creator)
            {
                return;
            }
        }
        Debug.Log(other.name);
        Destroy(this.gameObject);
        
    }

}
