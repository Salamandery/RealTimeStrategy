using Mirror;
using UnityEngine;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField]
    private Targeter targeter;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private float fireRange = 5f;
    [SerializeField]
    private float fireRate = 1f;
    [SerializeField]
    private float rotationSpeed = 20f;

    private float lastFireTime;

    [ServerCallback]
    void Update()
    {
        if (targeter.GetTarget() == null) { return; }
        if (!CanFireAtTarget()) { return; }

        FireProjectile();
    }

    private void FireProjectile()
    {
        Targetable target = targeter.GetTarget();
        Quaternion targetRotation = Quaternion.LookRotation(
            target.transform.position -
            transform.position
        );

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        if (Time.time > (1 / fireRate) + lastFireTime)
        {
            Quaternion projectileRotation = Quaternion.LookRotation(
                target.GetAimAtPoint().position -
                projectileSpawnPoint.position
            );
            GameObject projectileInstance = Instantiate(
                projectilePrefab,
                projectileSpawnPoint.position,
                projectileRotation
            );

            NetworkServer.Spawn(
                projectileInstance,
                connectionToClient
            );
            lastFireTime = Time.time;
        }
    }

    [Server]
    private bool CanFireAtTarget()
    {
        return (targeter.GetTarget().transform.position -
            transform.position).sqrMagnitude <=
            fireRange * fireRange;
    }
}