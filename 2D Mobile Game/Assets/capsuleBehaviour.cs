using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class capsuleBehaviour : BallBehaviour
{
    //Variable Declarations and Initializations
    int speed = 3;
    GameObject pps,kePs, nukePs, multiPs, laserPs, oneUpPs,expansionPs;
    
    
 
    // Start is called before the first frame update
    void Start()
    {
        pps = GameObject.Find("PowerUpPs");
        nukePs = GameObject.Find("MissileText");
        kePs = GameObject.Find("KEText");
        multiPs = GameObject.Find("Multi-Text");
        laserPs = GameObject.Find("LaserText");
        oneUpPs = GameObject.Find("1Up");
        expansionPs = GameObject.Find("ExpansionText");




    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.collider.name=="Paddle")
        {
            powerUpAudio.Play();
            Instantiate(pps, transform.position, Quaternion.identity);
            
            manifest();
            
            Destroy(gameObject);
            powerUps.Remove(powerUps.ElementAt(capsules.IndexOf(gameObject)));

            capsules.Remove(capsules.ElementAt(capsules.IndexOf(gameObject)));
            bonusPoints += powerUpBonus;
        }
    }
    public void manifest()
    {
        /* powerCode correspondence 
         * 1 = 1 up
         * 2 = multi-ball
         * 3 = expansion
         * 4 = kinetic energy
         * 5 = nuclear paddle
         * 6 = semi auto gun
        * */
        if (powerUps.ElementAt(capsules.IndexOf(gameObject)).getPowerCode() == 1)
        {
            lives += 1;
            Instantiate(oneUpPs, transform.position, Quaternion.identity);
        }
        else if (powerUps.ElementAt(capsules.IndexOf(gameObject)).getPowerCode() == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                ballClones.Add(Instantiate(ballClones.ElementAt(0), ballClones.ElementAt(0).transform.position, Quaternion.identity));
                activeBalls.Add(new Ball());
                ballCloneIdList.Add(ballClones.ElementAt(ballClones.Count - 1).GetComponent<BoxCollider2D>().GetInstanceID());

            }
            Instantiate(multiPs, transform.position, Quaternion.identity);

        }
        else if (powerUps.ElementAt(capsules.IndexOf(gameObject)).getPowerCode() == 3)
        {
            nullifyPowerUps();
            paddle.transform.localScale = new Vector3(4, 0.2968f, 1);
            Instantiate(expansionPs, transform.position, Quaternion.identity);

        }
        else if (powerUps.ElementAt(capsules.IndexOf(gameObject)).getPowerCode() == 4)
        {
            missileActivated = true;
            och.GetComponent<SpriteRenderer>().enabled = true;
            mch.GetComponent<SpriteRenderer>().enabled = true;
            ich.GetComponent<SpriteRenderer>().enabled = true;
            paddle.GetComponent<SpriteRenderer>().sprite = GameObject.Find("nuclear paddle").GetComponent<SpriteRenderer>().sprite;
            paddle.transform.localScale = new Vector3(1,1, 1);
            paddle.GetComponent<BoxCollider2D>().size = new Vector2(4.2f, 0.8f);
            healthBarOutline.GetComponent<SpriteRenderer>().enabled = false;
            healthBar.GetComponent<SpriteRenderer>().enabled = false;
            nukeTheme();
            Instantiate(nukePs, transform.position, Quaternion.identity);

        }
        else if(powerUps.ElementAt(capsules.IndexOf(gameObject)).getPowerCode() == 5)
        {
            lasersActivated = true;
            paddle.GetComponent<SpriteRenderer>().sprite = GameObject.Find("Semi-automatic paddle").GetComponent<SpriteRenderer>().sprite;
            paddle.transform.localScale = new Vector3(1.75f, 1.5f, 1);
            paddle.GetComponent<BoxCollider2D>().size = new Vector2(1.85f, 0.4f);
            Instantiate(laserPs, transform.position, Quaternion.identity);

        }
        else if (powerUps.ElementAt(capsules.IndexOf(gameObject)).getPowerCode() == 6)
        {
            KEActivated = true;
            resVel = 20;
            activeBalls.ElementAt(0).setBallSpeedY(resVel * Mathf.Sin(theta));
            activeBalls.ElementAt(0).setBallSpeedX(resVel * Mathf.Cos(theta));
            colPad.GetComponent<BoxCollider2D>().enabled = true;
            Instantiate(kePs, transform.position, Quaternion.identity);


        }
    }
}
