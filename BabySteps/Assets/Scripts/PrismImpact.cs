using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismImpact : MonoBehaviour
{
    public float initialStrength = 500f;
    public float strength = 500f;

    private void OnCollisionEnter(Collision collision)
    {
        float collisionForce = collision.impulse.y / Time.fixedDeltaTime;

        transform.localScale += Vector3.down * (collisionForce / strength);

        strength = (1 / transform.localScale.y) * initialStrength;

    }

}
