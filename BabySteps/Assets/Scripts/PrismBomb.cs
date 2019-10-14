using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismBomb : MonoBehaviour
{

    public GameObject explosion;

    public float explosionSize = 2f;
    public float explosionStrength = 2f;
    public float explosionFalloff = 10f;

    public float shockWaveSpeed = 1f;
    public float speedOfPrism = 0.1f;

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] prismsHit = Physics.OverlapSphere(transform.position, 20);


        foreach(Collider col in prismsHit)
        {
            float dist = Vector3.Distance(col.transform.position, transform.position);
            float amountToMove = ((explosionSize/ Mathf.Pow(dist, 2)) + explosionStrength) / explosionFalloff;

            PrismMoveInterface prism = col.GetComponent<PrismMoveInterface>();
            if (prism != null)
            {
                prism.Move(amountToMove, dist / shockWaveSpeed, speedOfPrism);
            }
        }
        Instantiate(explosion, transform.position, Quaternion.identity, null);
        Destroy(gameObject);
    }
}
