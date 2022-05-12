using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PaddleBehaviour :Levels
{
    //Variable Declarations and Initializations
   public static float leftVel ,rightVel;
   
    public static GameObject bg,ap,laser, cb ;
   
    Rigidbody2D rb3;
    public static AudioSource powerUpAudio, whoosh,laserEmission, laserImpact, laserImpact2;
    // Start is called before the first frame update
    void Start()
    {
        resVel = 12;
        rightVel = 10.0f;
        leftVel = -10.0f;
        bg = GameObject.Find("Background");
        ap = GameObject.Find("Acid Pool");
        rb3 = GetComponent<Rigidbody2D>();
        powerUpAudio = bg.GetComponent<AudioSource>();
        whoosh = missile.GetComponent<AudioSource>();
        laser = GameObject.Find("laser");
        laserEmission = laser.GetComponents<AudioSource>()[0];
        laserImpact = laser.GetComponents<AudioSource>()[1];
        laserImpact2 = laser.GetComponents<AudioSource>()[2];
        cb = GameObject.Find("CircleBurst");
       

    }
    //Class Constructor
    
    // Update is called once per frame
    void Update()
    {


    }
    void FixedUpdate()
    {
        ActivatePaddle();
        if (paddleActivated == true)
        {


            if (Input.GetAxis("Mouse X") > 0)
            {
                //transform.Translate(Vector2.right * rightVel * Time.deltaTime);
                rb3.MovePosition(transform.position + new Vector3(rightVel, 0.0f, 0.0f) * Time.deltaTime);
                if(teleportation==true&&Input.GetKeyDown("s"))
                {
                    transform.position= new Vector3(transform.position.x+3,transform.position.y, transform.position.z);
                }
            }
            else if (Input.GetAxis("Mouse X")<0)
            {
                //transform.Translate(Vector2.left * leftVel * Time.deltaTime);
                rb3.MovePosition(transform.position + new Vector3(leftVel, 0.0f, 0.0f) * Time.deltaTime);
                if (teleportation == true && Input.GetKeyDown("s"))
                {
                    transform.position = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z);

                }
            }

        }
        if(missileActivated==true&&missileLaunched==false)
        {
            if(Input.GetKey("f"))
            {
                missileLaunched = true;
                Instantiate(missile, transform.position, Quaternion.identity);
                whoosh.Play();
            }
        }
        
        if(lasersActivated==true&&Input.GetKey("f")&&lasersLaunched==false)
        {
            
            Instantiate(laser, new Vector3(transform.position.x - 0.475f, transform.position.y, transform.position.z), Quaternion.identity);
            Instantiate(laser, new Vector3(transform.position.x + 0.475f, transform.position.y, transform.position.z), Quaternion.identity);
            lasersLaunched = true;
            laserEmission.Play();
        }
    }
    public void resetPaddleVelocity()
    {
        leftVel = -10.0f;
        rightVel = 10.0f;
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.collider.name=="Left Wall")
        {
            leftVel = 0.0f;
            
        }
        if (collisionInfo.collider.name == "Right Wall" )
        {
            rightVel = 0.0f;
        }
        for(int i=0;i<ballClones.Count;i++)
        {
            if(collisionInfo.collider.GetInstanceID()==ballCloneIdList.ElementAt(i))
            {
             
                if (healthActivated == true)
                {
                    if (invincibility == false)
                    {
                        if (shieldActivated == false)
                        {
                            health -= 1;
                            healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x - originalHealthBarScale / dmgTol, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
                            healthBar.transform.localPosition = new Vector3(healthBar.transform.localPosition.x - 0.1183f, healthBar.transform.localPosition.y, healthBar.transform.localPosition.z);
                        }
                        else
                        {
                            shield -= 1;
                            shieldStatus.transform.localScale = new Vector3(shieldStatus.transform.localScale.x - originalShieldScale / shieldDmgTol, shieldStatus.transform.localScale.y, shieldStatus.transform.localScale.z);
                            shieldStatus.transform.localPosition = new Vector3(shieldStatus.transform.localPosition.x - 0.1028f, shieldStatus.transform.localPosition.y, shieldStatus.transform.localPosition.z);
                        }
                        if (shield == 1)
                        {
                            shieldActivated = false;
                            disableShield();
                        }
                    }
                    else
                    {
                        hitCount += 1;
                        Debug.Log(hitCount);
                        if(hitCount==44)
                        {
                            disableInvincibility();
                        }
                    }
                }
                if (transform.position.x < ballClones.ElementAt(i).transform.position.x)
                {
                    theta = Mathf.PI / 2 - ((ballClones.ElementAt(i).transform.position.x - transform.position.x) * Mathf.PI / 6);
                }
                else if (transform.position.x >ballClones.ElementAt(i).transform.position.x)
                {
                    theta = Mathf.PI / 2 + ((transform.position.x - ballClones.ElementAt(i).transform.position.x) * Mathf.PI / 6);
                }
                activeBalls.ElementAt(i).setBallSpeedX(resVel * Mathf.Cos(theta));
                activeBalls.ElementAt(i).setBallSpeedY(resVel * Mathf.Sin(theta));
            }
        }
    }
    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if (collisionInfo.collider.name == "Left Wall")
        {
            leftVel = -10.0f;
            
        }
        if (collisionInfo.collider.name == "Right Wall")
        {
            rightVel = 10.0f;
            
        }
        
    }
   
    public bool ActivatePaddle()
    {
        if(ballReleased==true)
        {
            
            paddleActivated = true;
            
            
        }
        else
        {
            paddleActivated = false;
        }
        return paddleActivated;
    }
    public void nukeTheme()
    {
        resVel =1;
        for(int i=0;i<ballClones.Count;i++)
        {
            activeBalls.ElementAt(i).setBallSpeedY(resVel * Mathf.Sin(theta));
            activeBalls.ElementAt(i).setBallSpeedX(resVel * Mathf.Cos(theta));
        }
        rightVel = 1.0f;
        leftVel = -1.0f;
        bg.GetComponent<SpriteRenderer>().material.color= new Vector4(0,0.5f,0,0.5f);
        ap.GetComponent<SpriteRenderer>().material.color = new Vector4(0, 0.5f, 0, 1);
    }
    public void ogTheme()
    {
        resVel = 12;
        for (int i = 0; i < ballClones.Count; i++)
        {
            activeBalls.ElementAt(i).setBallSpeedY(resVel * Mathf.Sin(theta));
            activeBalls.ElementAt(i).setBallSpeedX(resVel * Mathf.Cos(theta));
        }
        rightVel = 10.0f;
        leftVel = -10.0f;
        bg.GetComponent<SpriteRenderer>().material.color = new Vector4(0.0355f, 1, 0.0769f, 1);
        ap.GetComponent<SpriteRenderer>().material.color = new Vector4(0.0355f, 1, 0.0769f, 1);
        paddle.GetComponent<SpriteRenderer>().sprite = ogPadSprite;
        paddle.transform.localScale = originalPaddleScale;
        paddle.transform.position = new Vector3(paddle.transform.position.x, -6.84f, paddle.transform.position.z);
        paddle.GetComponent<BoxCollider2D>().size = new Vector2(1,1);
    }
}
