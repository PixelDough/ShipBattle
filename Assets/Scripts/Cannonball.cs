using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cannonball : MonoBehaviour
{

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        FindObjectOfType<ScreenShake>().Shake();
        StartCoroutine(CannonLife());
    }


    private void Update()
    {
        if (transform.position.y < -2f) Destroy(gameObject);
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


    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.attachedRigidbody)
    //    {
            

    //        if (other.attachedRigidbody.GetComponent<WaterToy>())
    //        {
    //            other.attachedRigidbody.GetComponent<WaterToy>().Explode();
    //        }

    //    }

    //    GetComponent<WaterToy>().Explode();
        
    //}

}
