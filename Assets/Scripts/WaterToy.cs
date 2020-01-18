using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterToy : MonoBehaviour
{

    private float UpwardForce = 12.72f * 2f; // 9.81 is the opposite of the default gravity, which is 9.81. If we want the boat not to behave like a submarine the upward force has to be higher than the gravity in order to push the boat to the surface
    [HideInInspector] public bool isInWater = false;
    public bool floatOnWater = true;
    public bool explodeOnCollision = false;

    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }


    private void Update()
    {

        if (transform.position.y < 0)
        {
            isInWater = true;
        }
        else
        {
            isInWater = false;
        }

    }

    private void OnDestroy()
    {
        
    }

    private void FixedUpdate()
    {

        if (isInWater && floatOnWater)
        {
            Vector3 vel = rb.velocity;

            if (vel.y < 0)
            {
                if (rb.velocity.y < 0) vel.y *= 0.5f;
                rb.velocity = vel;
            }

            // apply upward force
            Vector3 force = Vector3.up * UpwardForce;
            rb.AddForce(force, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explodeOnCollision)
        {
            Explode();
        }
    }

    public void Explode()
    {
        FindObjectOfType<ScreenShake>().Shake();
        Object explosionParticle = Resources.Load("Explosion Particle");
        Instantiate(explosionParticle, transform.position + Vector3.up * 0.2f, Quaternion.identity);
        Destroy(this.gameObject);
    }

}
