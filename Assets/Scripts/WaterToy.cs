using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterToy : MonoBehaviour
{

    private float UpwardForce = 12.72f * 2f; // 9.81 is the opposite of the default gravity, which is 9.81. If we want the boat not to behave like a submarine the upward force has to be higher than the gravity in order to push the boat to the surface
    [HideInInspector] public bool isInWater = false;
    public bool floatOnWater = true;
    private List<Rigidbody> ignoreCollisionsList = new List<Rigidbody>();
    public bool explodeOnCollision = false;
    public Object deathParticle = null;

    private Rigidbody rb;

    private bool wasInWaterPreviousFrame = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.angularDrag = 1f;
        rb.drag = 1f;

        int floatSeed = Random.Range(0, 100);
        foreach(Renderer r in gameObject.GetComponentsInChildren<Renderer>())
        {
            r.material.SetFloat("_FloatSeed", floatSeed);
            
        }

        if (deathParticle == null) deathParticle = Resources.Load("Explosion Particle");

    }


    private void Update()
    {

        if (transform.position.y < 0)
        {
            isInWater = true;

            if (wasInWaterPreviousFrame && rb.velocity.y < -5)
            {
                //Object splash = Resources.Load("Splash Particle");
                //Instantiate(splash, transform.position + Vector3.up * 0.2f, Quaternion.identity);
            }
            wasInWaterPreviousFrame = false;
        }
        else
        {
            isInWater = false;
            wasInWaterPreviousFrame = true;
        }

    }


    private void OnDestroy()
    {
        //foreach(ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        //{
        //    ps.gameObject.transform.parent = null;
        //    ParticleSystem.EmissionModule emission = ps.emission;
        //    emission.rateOverTime = 0;
        //    emission.rateOverDistance = 0;
        //    emission.burstCount = 0;

        //    ParticleSystem.MainModule main = ps.main;
        //    main.stopAction = ParticleSystemStopAction.Destroy;
        //}
        //foreach(TrailRenderer t in GetComponentsInChildren<TrailRenderer>())
        //{
        //    t.transform.parent = null;
        //    t.autodestruct = true;
        //    Destroy(t.gameObject, t.time);
        //}
    }


    public void SetIgnoreCollisions(Rigidbody _rigidbody, bool _state = true)
    {
        foreach (Collider c1 in GetComponentsInChildren<Collider>())
        {
            foreach (Collider c2 in _rigidbody.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(c1, c2, _state);
            }
        }
    }


    public void ResetIgnoreCollisions(Rigidbody _rigidbody, float _waitTime = 0f)
    {
        StartCoroutine(_ResetIgnoreCollisions(_rigidbody, _waitTime));
    }


    private IEnumerator _ResetIgnoreCollisions(Rigidbody _rigidbody, float _waitTime = 0f)
    {
        yield return new WaitForSeconds(_waitTime);

        foreach (Collider c1 in GetComponentsInChildren<Collider>())
        {
            foreach (Collider c2 in _rigidbody.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(c1, c2, false);
            }
        }

        yield return null;
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
            if (rb.position.y < -0.2f)
            {
                Vector3 force = Vector3.up * UpwardForce;
                rb.AddForce(force, ForceMode.Acceleration);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreCollisionsList.Contains(collision.collider.attachedRigidbody)) return;

        if (explodeOnCollision)
        {
            Explode();
        }
    }

    public void Explode()
    {
        FindObjectOfType<ScreenShake>().Shake();
        Instantiate(deathParticle, transform.position + Vector3.up * 0.2f, Quaternion.identity);
        Destroy(this.gameObject);
    }

}
