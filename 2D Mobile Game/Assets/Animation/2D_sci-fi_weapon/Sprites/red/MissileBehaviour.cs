using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissileBehaviour : BallBehaviour
{
    //Variable Declarations and Initializations
    float launchSpeed;
    Rigidbody2D rBody;
    float deltaX, deltaY, alpha, lsx, lsy;
    Vector3 rotation;
    int rotationSpeed = 360;
    bool stop = false;
    public static GameObject exp, inc;
    public static AudioSource explosion, incineration;
    // Start is called before the first frame update
    void Start()
    {
        launchSpeed = 10.0f;
        rBody = GetComponent<Rigidbody2D>();
        rotation = transform.rotation.eulerAngles;
        exp = GameObject.Find("Explosion");
        explosion = exp.GetComponent<AudioSource>();
        inc = GameObject.Find("Incineration");
        incineration = inc.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
            deltaX = Mathf.Abs(transform.position.x - mch.transform.position.x);
            deltaY = Mathf.Abs(transform.position.y - mch.transform.position.y);
            if (mch.transform.position.y >= transform.position.y)
            {
                if (mch.transform.position.x > transform.position.x)
                {
                   alpha = Mathf.Atan(deltaY / deltaX);
                    // transform.eulerAngles = new Vector3(0, 0, radians2Degrees( -alpha));

                }
                else if (mch.transform.position.x < transform.position.x)
                {
                    alpha = Mathf.PI - Mathf.Atan(deltaY / deltaX);
                    //transform.eulerAngles = new Vector3(0, 0, radians2Degrees(Mathf.PI - alpha));

                }
                else
                {
                    alpha = Mathf.PI / 2;

                }
            }
           
            
            
            transform.eulerAngles = new Vector3(0, 0, radians2Degrees(alpha - Mathf.PI / 2));
            lsx = launchSpeed * Mathf.Cos(alpha);
            lsy = launchSpeed * Mathf.Sin(alpha);
            
            rBody.AddForce(new Vector3(lsx, lsy, 0), ForceMode2D.Force);
        
    }
    float radians2Degrees(float a)
    {
        return (a * 180) / Mathf.PI;
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.collider.tag == "target" || collisionInfo.collider.tag == "boundary")
        {

            Instantiate(exp, transform.position, Quaternion.identity);
            Destroy(gameObject);
            missileActivated = false;
            missileLaunched = false;
            explosion.Play();
            ogTheme();
            //nullifyPowerUps();
          
        }
        
    }
}
