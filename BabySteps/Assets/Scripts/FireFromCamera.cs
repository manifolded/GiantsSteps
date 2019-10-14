using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFromCamera : MonoBehaviour
{

    public GameObject projectile;
    public float fireDelay;
    public float force;

    private bool canFire = true;
    private bool firing = false;

    private void Start()
    {
        StartCoroutine(GunHandler());
    }

    private void Update()
    {
        firing = Input.GetKeyDown(KeyCode.Space);
    }

    IEnumerator GunHandler()
    {
        while (true)
        {
            if (canFire)
            {
                if (firing)
                {
                    Debug.Log("fire");
                    GameObject currentProjectile = Instantiate(projectile, transform.position, Quaternion.identity, null);
                    currentProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.VelocityChange);
                    canFire = false;
                }
                yield return new WaitForEndOfFrame();
            }
            else
            {
                yield return new WaitForSeconds(fireDelay);
                canFire = true;
            }

        }
    }
}
