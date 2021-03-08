using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    public float speed;
    public bool initRandom;
    public float direction;
    public float limits;

    void Start()
    {
        transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 359));
        if (Random.Range(0, 1) == 0)  direction = 1;  else direction = -1;

    }
    void Update()
    {
        Vector3 rot = transform.eulerAngles;      

        if (limits > 0)
        {
            float rot_z = rot.z + 360;
            if (rot_z > limits + 360 || rot_z < -limits + 360)
                direction *= -1;
        }
        rot.z += speed * Time.deltaTime * direction;
        transform.eulerAngles = rot;
    }
}
