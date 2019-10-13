using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatScalingEffect : MonoBehaviour
{

    public float Delay = 0.01f;
    public float ScaleAmount = 0.005f;
    public float SinScaling = 0.5f;

    private float offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = Random.Range(-10.0f, 10.0f);
        StartCoroutine(ScaleEffect());
    }

    IEnumerator ScaleEffect()
    {
        while (true)
        {
            float yScale = Mathf.Lerp(-ScaleAmount, ScaleAmount, (Mathf.Sin((Time.realtimeSinceStartup + offset) * SinScaling) + 1) / 2);

            transform.localScale += Vector3.up * yScale;

            yield return new WaitForSeconds(Delay);
        }
    }
}
