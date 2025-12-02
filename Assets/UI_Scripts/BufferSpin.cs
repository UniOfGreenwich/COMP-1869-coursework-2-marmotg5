using UnityEngine;

public class BufferSpin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float spinspeed = 100f; // speed of the buffer


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f,0f,-spinspeed * Time.deltaTime); //roation of the buffer 
    }
}
// this  script controlls the roation for the buffer UI on the loading screen :)