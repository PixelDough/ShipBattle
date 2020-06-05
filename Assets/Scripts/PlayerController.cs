using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public GameManager.PlayerData playerData;

    public GameObject cannonballPrefab;
    public WaterToy powderKegPrefab;
    public Transform shipModel;
    public Transform poleModel;

    [Header("Particles")]
    public ParticleSystem[] waterParticles;
    bool waterParticlesPlaying = false;

    Player player;
    Rigidbody rb;

    WaterToy waterToy;


    private void Start()
    {
        player = ReInput.players.GetPlayer(playerData.playerID);
        rb = GetComponent<Rigidbody>();
        waterToy = GetComponent<WaterToy>();

        //FindObjectOfType<Cinemachine.CinemachineTargetGroup>().AddMember(transform, 1, 5);

        //shipModel.GetComponentInChildren<Renderer>().material.SetColor("_BaseColor", Random.ColorHSV(0, 1, 1, 1, .5f, .5f));
        shipModel.GetComponentInChildren<Renderer>().material.SetColor("_BaseColor", GameManager.Instance.shipTypes[playerData.playerID].color);
        /*
         * Color Choices:
         * 801400
         */

        
    }


    private void Update()
    {

        float ySpeed = rb.velocity.y;

        if (waterToy.isInWater)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * 4f, 0.1f);

            if (!waterParticlesPlaying)
            {
                foreach(ParticleSystem ps in waterParticles)
                {
                    ps.Play();
                }
                waterParticlesPlaying = true;
            }
        }
        else
        {
            //Debug.Log("NOT IN WATER!!!");
            if (waterParticlesPlaying)
            {
                foreach (ParticleSystem ps in waterParticles)
                {
                    ps.Stop();
                }
                waterParticlesPlaying = false;
            }
        }

        rb.velocity = new Vector3(rb.velocity.x, ySpeed, rb.velocity.z);

        rb.MoveRotation(Quaternion.Euler(0, rb.rotation.eulerAngles.y + (100 * player.GetAxis(RewiredConsts.Action.Turn)) * Time.deltaTime, 0));
        shipModel.localRotation = Quaternion.Lerp(shipModel.localRotation, Quaternion.Euler(shipModel.localRotation.x, shipModel.localRotation.y, -player.GetAxis(RewiredConsts.Action.Turn) * 15f), 0.1f);

        if (player.GetAxis(RewiredConsts.Action.Shoot) != 0 && player.GetAxisPrev(RewiredConsts.Action.Shoot) == 0)
        {
            Vector3 cbDirection = transform.right;
            if (player.GetAxis(RewiredConsts.Action.Shoot) < 0) cbDirection = -transform.right;

            if (transform.eulerAngles.y < 270 && transform.eulerAngles.y > 90)
            {
                //cbDirection = cbDirection == transform.right ? -transform.right : transform.right;
            }

            Cannonball cb = Instantiate(cannonballPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity).GetComponent<Cannonball>();
            cb.GetComponent<WaterToy>().SetIgnoreCollisions(rb, true);
            cb.GetComponent<WaterToy>().ResetIgnoreCollisions(rb, 1f);
            cb.transform.LookAt(cb.transform.position + cbDirection);
            
        }

        if (player.GetButtonDown(RewiredConsts.Action.Bomb))
        {
            WaterToy bomb = Instantiate(powderKegPrefab, transform.position + Vector3.up * 1f - transform.forward * 1f, transform.rotation);
            bomb.GetComponent<WaterToy>().SetIgnoreCollisions(rb, true);
            bomb.GetComponent<WaterToy>().ResetIgnoreCollisions(rb, 1f);
            bomb.GetComponent<Rigidbody>().AddForce(-transform.forward * 300f + transform.up * 400f);
            waterToy.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            //bomb.transform.LookAt(bomb.transform.position + Vector3.down);
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        //rb.AddForce(Vector3.Scale(collision.contacts[0].normal, new Vector3(1, 0, 1) * 50f), ForceMode.VelocityChange);
        //Debug.Log(Vector3.Scale(collision.contacts[0].normal, new Vector3(1, 0, 1) * 50f));
    }

}
