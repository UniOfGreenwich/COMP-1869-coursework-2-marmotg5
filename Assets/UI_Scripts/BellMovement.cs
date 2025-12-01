using UnityEngine;

public class BellMovement : MonoBehaviour
{
    public float angle = 6f; // the angle of movement 
    public float speed = 5f;// speed of movement 

    void Update()
    {
        float Jiggle = Mathf.Sin(Time.time * speed) * angle; //this makes the bell wiggle back and forth
        transform.rotation = Quaternion.Euler(0, 0, Jiggle); // which axis the bell moves on 
    }
}

