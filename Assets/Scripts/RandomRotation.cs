using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    public float speed;
    public bool initRandom;
    public float direction;
    public float timeToChangeDirection;
    float timer;

    void Start()
    {
        transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 359));
        if (Random.Range(0, 1) == 0)  direction = 1;  else direction = -1;
    }
    void Update()
    {
        Vector3 rot = transform.eulerAngles;
        rot.z += speed * Time.deltaTime * direction;
        float rot_z = transform.rotation.eulerAngles.z;

        if (timeToChangeDirection > 0)
        {
            if (timeToChangeDirection > 0)
            {
                timer += Time.deltaTime;
                if (timer > timeToChangeDirection)
                {
                    timer = 0;
                    direction *= -1;
                }
            }   
        }
        transform.eulerAngles = rot;
    }
}
