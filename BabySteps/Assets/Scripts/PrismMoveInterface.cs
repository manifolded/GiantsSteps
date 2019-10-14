using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismMoveInterface : MonoBehaviour
{


    public void Move(float amountToMove, float delay, float speed)
    {
        StartCoroutine(MovePrism(amountToMove, delay, speed));
    }

    IEnumerator MovePrism(float newPos, float delay, float speed)
    {
        //Debug.Log(newPos + ": new pos");
        float lerper = 0;
        float lastYPos = transform.localScale.y;

        yield return new WaitForSeconds(delay - 0.6f);

        while (lerper < 1)
        {
            //Debug.Log("Moving");

            transform.localScale = new Vector3(1, Mathf.Lerp(lastYPos, -newPos + lastYPos, lerper), 1);
            lerper += speed;

            yield return new WaitForFixedUpdate();
        }

        if(transform.localScale.y <= 0)
        {
            Destroy(gameObject);
        }
        
    }
}
