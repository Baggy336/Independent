using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// This script handles charging a shot, firing a shot, and spawning the projectile with force
public class FireProjectile : NetworkBehaviour
{
    public Rigidbody projectile;
    public Transform projectileOrigin;
    public float minimumLaunch = 15f;
    public float maximumLaunch = 30f;
    public float maximumCharge = .75f;

    private string fireInput;
    private float previousLaunchForce;
    private float chargeSpeed;
    private bool hasFired;

    private void OnEnable()
    {
        previousLaunchForce = minimumLaunch;
    }

    private void Start()
    {
        fireInput = "Fire";
        chargeSpeed = (maximumLaunch - minimumLaunch) / maximumCharge;
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (previousLaunchForce >= maximumLaunch && !hasFired)
        {
            previousLaunchForce = maximumLaunch;
            TryShootServerRpc();
            Shoot();
        }
        else if (Input.GetButtonDown(fireInput))
        {
            hasFired = false;
            previousLaunchForce = minimumLaunch;
        }
        else if (Input.GetButtonDown(fireInput) && !hasFired)
        {
            previousLaunchForce += chargeSpeed * Time.deltaTime;
        }
        else if (Input.GetButtonUp(fireInput) && !hasFired)
        {
            TryShootServerRpc();
            Shoot();
        }
    }

    // If this is the host, try to go through the client (creates some host lag to match the client)
    [ServerRpc]
    private void TryShootServerRpc()
    {
        TryShootClientRpc();
    }

    // If this is the client, or called from the host. Try to fire
    [ClientRpc]
    private void TryShootClientRpc()
    {
        if (!IsOwner)
        Shoot();
    }

    // Handle the spawning and math for launching the projectile
    private void Shoot()
    {
        hasFired = true;

        Rigidbody bulletInst = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation) as Rigidbody;
        bulletInst.GetComponentInChildren<NetworkObject>().Spawn(true);
        bulletInst.velocity = previousLaunchForce * projectileOrigin.forward;

        previousLaunchForce = minimumLaunch;
    }
}
