using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairBehaviour : Levels
{
    //Variable Declarations and Initializations
   
    int rotationSpeed = 180, movementSpeed = 10;
    bool contact; 
    // Start is called before the first frame update
    void Start()
    {
        
        GetComponent<SpriteRenderer>().enabled = false;
        mch.GetComponent<SpriteRenderer>().enabled = false;
        ich.GetComponent<SpriteRenderer>().enabled = false;
        contact = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(missileActivated==false)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            mch.GetComponent<SpriteRenderer>().enabled = false;
            ich.GetComponent<SpriteRenderer>().enabled = false;
        }
        
            transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime, Space.World);
            mch.transform.Rotate(Vector3.forward * rotationSpeed/2 * Time.deltaTime, Space.World);
        if (missileActivated == true&&missileLaunched==false)
        {
            if (Input.GetAxis("Mouse X") > 0)
            {
                transform.Translate(Vector2.right * movementSpeed * Time.deltaTime, Space.World);
                
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                transform.Translate(Vector2.left * movementSpeed * Time.deltaTime, Space.World);
               
            }
            if (Input.GetAxis("Mouse Y") > 0)
            {
                transform.Translate(Vector2.up * movementSpeed * Time.deltaTime, Space.World);
                
            }
            if (Input.GetAxis("Mouse Y") < 0&&transform.position.y>=paddle.transform.position.y)
            {
                transform.Translate(Vector2.down * movementSpeed * Time.deltaTime, Space.World);
               
            }
            mch.transform.position = transform.position;
        }
        if(missileLaunched==true)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = missile.GetComponent<Rigidbody2D>().velocity;
        }
    }
   

}
