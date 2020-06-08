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

    public int maxHealth = 1;
    private int health;

    [Header("Fire")]
    public bool explodeOnCollision = false;
    public LayerMask ignoreExplosionWhenCollidingLayers;
    public bool combustable = false;
    public float combustionTime = 5f;
    private bool isOnFire = false;

    public Object deathParticle = null;

    private Rigidbody rb;

    private bool wasInWaterPreviousFrame = false;

    bool isWrappingX = false;
    bool isWrappingY = false;
    Renderer[] renderers;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.angularDrag = 1f;
        rb.drag = 1f;

        health = maxHealth;

        int floatSeed = Random.Range(0, 100);
        foreach(Renderer r in gameObject.GetComponentsInChildren<Renderer>())
        {
            r.material.SetFloat("_FloatSeed", floatSeed);
            
        }

        if (deathParticle == null) deathParticle = Resources.Load("Explosion Particle");

        renderers = GetComponentsInChildren<Renderer>();

    }


    private void Update()
    {
        //ScreenWrap();
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


    public void Damage(int _amount = 1)
    {
        health -= _amount;

        if (health <= 0)
        {
            Explode();
        }
        else if (health == 1)
        {
            if (maxHealth > 1)
            {
                //Renderer[] renderers = GetComponentsInChildren<Renderer>();
                //foreach(Renderer r in renderers)
                //{
                //    foreach(Material m in r.materials)
                //    {
                //        m.SetFloat("_Flashing", 1);
                //    }
                //}

            }
        }
    }


    public void Destroy()
    {
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
            if (!ps.gameObject.activeSelf) { continue; }

            ParticleSystem.MainModule main = ps.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            main.loop = false;

            ParticleSystem.EmissionModule emission = ps.emission;
            emission.rateOverTime = 0;
            emission.rateOverDistance = 0;
            emission.burstCount = 0;

            ps.transform.parent = null;
        }
        particleSystems = null;

        foreach (TrailRenderer t in GetComponentsInChildren<TrailRenderer>())
        {
            if (!t.gameObject.activeSelf) { continue; }

            t.autodestruct = true;
            Destroy(t.gameObject, t.time);
            t.transform.parent = null;
        }

        Destroy(this.gameObject);
    }


    bool CheckRenderers()
    {
        foreach (var renderer in renderers)
        {
            // If at least one render is visible, return true
            if (renderer.isVisible)
            {
                return true;
            }
        }

        // Otherwise, the object is invisible
        return false;
    }


    void ScreenWrap()
    {
        Vector3 originalPosition = transform.position;
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPosition.x > 1)
        {
            Vector3 e = Camera.main.ViewportToWorldPoint(new Vector3(0, screenPosition.y, screenPosition.z));
            transform.position = new Vector3(e.x, transform.position.y, transform.position.z);
        }
        if (screenPosition.x < 0)
        {
            Vector3 e = Camera.main.ViewportToWorldPoint(new Vector3(1, screenPosition.y, screenPosition.z));
            transform.position = new Vector3(e.x, transform.position.y, transform.position.z);
        }
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

            //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, transform.eulerAngles.y, 0f), 5f * Time.deltaTime);
        }

        //ScreenWrap();
    }


    public void SetOnFire()
    {
        if (!isOnFire)
        {
            StartCoroutine(_SetOnFire());
        }
    }


    private IEnumerator _SetOnFire()
    {

        isOnFire = true;

        yield return new WaitForSeconds(combustionTime);

        isOnFire = false;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreCollisionsList.Contains(collision.collider.attachedRigidbody)) return;

        if (ignoreExplosionWhenCollidingLayers.value != 1 << collision.gameObject.layer)
        {


            //rb.AddExplosionForce(2000f, collision.contacts[0].point, 1f);
            //return;

            if (explodeOnCollision)
            {
                Damage();
            }
        }
    }

    public virtual void Explode()
    {

        //Collider[] others = Physics.OverlapSphere(transform.position, 2f);
        //foreach (Collider c in others)
        //{
        //    WaterToy _waterToy;
        //    if (c.attachedRigidbody)
        //    {
        //        if (c.attachedRigidbody.gameObject.TryGetComponent(out _waterToy))
        //        {
        //            _waterToy.Explode(0.5f);
        //        }
        //    }
        //}

        FindObjectOfType<ScreenShake>().Shake();
        Instantiate(deathParticle, transform.position + Vector3.up * 0.2f, Quaternion.identity);
        Destroy();
    }

    public void Explode(float _time)
    {
        StartCoroutine(_Explode(_time));
    }

    private IEnumerator _Explode(float _time)
    {
        yield return new WaitForSeconds(_time);

        Explode();
    }

}
