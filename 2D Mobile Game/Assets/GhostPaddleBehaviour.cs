using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPaddleBehaviour : MonoBehaviour
{
    int speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x<=46)
        {
            speed = -5;
        }
        if(transform.position.x>=53.7)
        {
            speed = 5;
        }
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
