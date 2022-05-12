using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FirstGearGames.SmoothCameraShaker;
public class BallBehaviour : PaddleBehaviour
{
    //Variable Declarations and Initializations
    GameObject lw;
    GameObject rw;
    public Rigidbody2D rb;
    Rigidbody2D rb2;
    public static GameObject floor, goldCoin;
    public static GameObject ps, sapphirePs, emeraldPs, rubyPs, diamondPs, fbs, cb2;
    public static AudioSource blockHit, poweredBlockHit,ballTap, wallHit, turnLost;
    public ShakeData sd, sd2;
    public static bool verticalBlockCol = false, horizontalBlockCol = false;
    // Start is called before the first frame update
    void Start()
    {
        floor = GameObject.Find("Floor");
        goldCoin = GameObject.Find("CoinGold-$");
        rb = GetComponent<Rigidbody2D>();
        rb2 = paddle.GetComponent<Rigidbody2D>();
       
        //Particle System references
        ps = GameObject.Find("Particle System");
        sapphirePs = GameObject.Find("Sapphire Particle System");
        emeraldPs = GameObject.Find("Emerald Particle System ");
        rubyPs = GameObject.Find("Ruby Particle System ");
        diamondPs = GameObject.Find("Diamond Particle System");
        //

        fbs = GameObject.Find("fireBallStrike");
        cb2 = GameObject.Find("CircleBurst_v2");
       
        //AudioSource references
        blockHit = primaryBlock.GetComponent<AudioSource>();
        ballTap = paddle.GetComponent<AudioSource>();
        wallHit = GetComponents<AudioSource>()[0];
        poweredBlockHit = GetComponents<AudioSource>()[1];
        turnLost = floor.GetComponent<AudioSource>();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        releaseBall();
        //transform.eulerAngles = new Vector3(0, 0, theta);
        for (int i = 0; i < activeBalls.Count; i++)
        {
            activeBalls.ElementAt(i).setBallVelX(new Vector3(activeBalls.ElementAt(i).getBallSpeedX(), 0, 0));
            activeBalls.ElementAt(i).setBallVelY(new Vector3(0, activeBalls.ElementAt(i).getBallSpeedY(), 0));

        }

        if (ballReleased == true)
        {

            for (int i = 0; i < activeBalls.Count; i++)
            {
                if (activeBalls.ElementAt(i).getVerticalBlockCol() == true)
                {
                    activeBalls.ElementAt(i).setBallSpeedY(-activeBalls.ElementAt(i).getBallSpeedY());
                    activeBalls.ElementAt(i).setVerticalBlockCol(false);
                }
                if (activeBalls.ElementAt(i).getHorizontalBlockCol() == true)
                {
                    activeBalls.ElementAt(i).setBallSpeedX(-activeBalls.ElementAt(i).getBallSpeedX());
                    activeBalls.ElementAt(i).setHorizontalBlockCol(false);
                }
            }


            for (int i = 0; i < ballClones.Count; i++)
                ballClones.ElementAt(i).GetComponent<Rigidbody2D>().MovePosition(ballClones.ElementAt(i).transform.position + (activeBalls.ElementAt(i).getBallVelX() + activeBalls.ElementAt(i).getBallVelY()) * Time.deltaTime);

            //Check for kinetic Energy Power-up
            if (KEActivated == true)
            {

                ballClones.ElementAt(0).GetComponent<SpriteRenderer>().material.color = Color.Lerp(Color.white, Color.blue, Mathf.PingPong(Time.time, 1));
                
                
            }
            else
            {
                ballClones.ElementAt(0).GetComponent<SpriteRenderer>().material.color = Color.white;
            }
        }
    }
        void OnCollisionEnter2D(Collision2D collisionInfo)
        {



            if ((collisionInfo.collider.name == "Paddle"||collisionInfo.collider.name=="ColliderPadding") && ballReleased == true)
            {
                ballTap.Play();
                Instantiate(cb2, transform.position, Quaternion.identity);
            }
       
            if (collisionInfo.collider.name == "Floor")
            {
                if (ballClones.Count > 1)
                {
                    for (int i = 0; i < ballClones.Count; i++)
                    {
                        if (collisionInfo.otherCollider.GetInstanceID() == ballCloneIdList.ElementAt(i))
                        {
                            Destroy(ballClones.ElementAt(i));
                            ballClones.Remove(ballClones.ElementAt(i));
                            ballCloneIdList.Remove(ballCloneIdList.ElementAt(i));
                            activeBalls.Remove(activeBalls.ElementAt(i));
                        }
                    }
                }
                else
                {
                    turnLost.Play();
                    ballReleased = false;
                    paddleActivated = false;
                    lives -= 1;
                    resetBallPos();
                    paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
                    if (missileActivated == true)
                    {
                        missileActivated = false;
                        ogTheme();
                    }
                    if (lasersActivated == true)
                    {
                        lasersActivated = false;
                        ogTheme();
                    }
                if (KEActivated == true)
                {
                    KEActivated = false;
                    resVel = 12;
                    activeBalls.ElementAt(0).setBallSpeedY(resVel * Mathf.Sin(theta));
                    activeBalls.ElementAt(0).setBallSpeedX(resVel * Mathf.Cos(theta));
                    colPad.GetComponent<BoxCollider2D>().enabled = false;
                }
            }


            }
            //Collision detection for regular blocks
            for (int i = 0; i < idList.Count; i++)
            {
                if (collisionInfo.collider.GetInstanceID() == idList.ElementAt(i))
                {

                    CameraShakerHandler.Shake(sd);
                   
                    Instantiate(ps, blocks.ElementAt(i).transform.position, Quaternion.identity);

                for (int j = 0; j < ballClones.Count; j++)
                    {
                        if (collisionInfo.otherCollider.GetInstanceID() == ballCloneIdList.ElementAt(j)&&KEActivated==false)
                        {
                            if(ballClones.Count>1)
                            {
                                bonusPoints += allObjects.ElementAt(i).getNetPointVal() * ballClones.Count;
                            }
                            if (transform.position.y > (blocks.ElementAt(i).transform.position.y + (blocks.ElementAt(i).transform.localScale.y / 2)) || transform.position.y < (blocks.ElementAt(i).transform.position.y - (blocks.ElementAt(i).transform.localScale.y / 2)))
                            {


                                activeBalls.ElementAt(j).setVerticalBlockCol(true);

                            }
                            else
                            {

                                activeBalls.ElementAt(j).setHorizontalBlockCol(true);

                            }
                        }
                    }
                    Destroy(blocks.ElementAt(i));
                    blocks.Remove(blocks.ElementAt(i));
                    idList.Remove(idList.ElementAt(i));
                    pointIncrease = 10;
                    basePoints += pointIncrease;
                    if(KEActivated==false)
                    {
                    blockHit.Play();
                    }
                    else if(KEActivated==true)
                    {
                    poweredBlockHit.Play();
                    bonusPoints += KEBonus;

                    
                    }

                    break;
                }
            }

            //Collision detection for sapphire blocks
            for (int i = 0; i < sapphireIdList.Count; i++)
            {
                if (collisionInfo.collider.GetInstanceID() == sapphireIdList.ElementAt(i) && KEActivated == false)
                {

                    int frames = 0;
                    while (frames == 0)
                    {
                    
                    if (transform.position.y > (sapphireBlocks.ElementAt(i).transform.position.y + (sapphireBlocks.ElementAt(i).transform.localScale.y / 2)) || transform.position.y < (sapphireBlocks.ElementAt(i).transform.position.y - (sapphireBlocks.ElementAt(i).transform.localScale.y / 2)))
                        {
                            activeBalls.ElementAt(ballClones.IndexOf(gameObject)).setVerticalBlockCol(true);


                        }
                        else
                        {
                            activeBalls.ElementAt(ballClones.IndexOf(gameObject)).setHorizontalBlockCol(true);
                        }
                        frames += 1;
                    }
                    break;
                }
            }
            for (int i = 0; i < sapphireBlocks.Count; i++)
            {
                if (collisionInfo.collider.GetInstanceID() == sapphireIdList.ElementAt(i))
                {
                if (KEActivated == false)
                {
                    blockHit.Play();
                    if (sapphireObjects.ElementAt(i).getHitCount() == 1)
                    {
                        CameraShakerHandler.Shake(sd);
                        Instantiate(sapphirePs, sapphireBlocks.ElementAt(i).transform.position, Quaternion.identity);
                        Destroy(sapphireBlocks.ElementAt(i));
                        sapphireBlocks.Remove(sapphireBlocks.ElementAt(i));
                        sapphireObjects.Remove(sapphireObjects.ElementAt(i));
                        sapphireShakers.Remove(sapphireShakers.ElementAt(i));
                        sapphireIdList.Remove(sapphireIdList.ElementAt(i));
                        pointIncrease = 10;
                        basePoints += pointIncrease;

                    }
                    else
                    {
                        
                        sapphireShakers.ElementAt(i).enabled = true;
                        sapphireObjects.ElementAt(i).setHitCount(sapphireObjects.ElementAt(i).getHitCount() + 1);
                        pointIncrease = 5;
                        basePoints += pointIncrease;
                    }
                }
                else
                {
                    if (ballClones.Count > 1)
                    {
                        bonusPoints += allObjects.ElementAt(i + blocks.Count).getNetPointVal() * ballClones.Count;
                    }

                    poweredBlockHit.Play();
                    Instantiate(sapphirePs, sapphireBlocks.ElementAt(i).transform.position, Quaternion.identity);
                    
                    Destroy(sapphireBlocks.ElementAt(i));
                    sapphireBlocks.Remove(sapphireBlocks.ElementAt(i));
                    sapphireObjects.Remove(sapphireObjects.ElementAt(i));
                    sapphireShakers.Remove(sapphireShakers.ElementAt(i));
                    sapphireIdList.Remove(sapphireIdList.ElementAt(i));
                    pointIncrease = allObjects.ElementAt(i+blocks.Count).getNetPointVal()-(allObjects.ElementAt(i+blocks.Count).getHitCount()*5);
                    Debug.Log(pointIncrease);
                    basePoints += pointIncrease;
                    bonusPoints += KEBonus;
                }
                
                
                    break;
                }
            }
            //Collision detection for emerald blocks
            for (int i = 0; i < emeraldIdList.Count; i++)
            {
                if (collisionInfo.collider.GetInstanceID() == emeraldIdList.ElementAt(i) && KEActivated == false)
                {


                    
                    int frames = 0;
                    while (frames == 0)
                    {
                    
                    if (transform.position.y > (emeraldBlocks.ElementAt(i).transform.position.y + (emeraldBlocks.ElementAt(i).transform.localScale.y / 2)) || transform.position.y < (emeraldBlocks.ElementAt(i).transform.position.y - (emeraldBlocks.ElementAt(i).transform.localScale.y / 2)))
                        {

                            activeBalls.ElementAt(ballClones.IndexOf(gameObject)).setVerticalBlockCol(true);
                        }
                        else
                        {
                            activeBalls.ElementAt(ballClones.IndexOf(gameObject)).setHorizontalBlockCol(true);
                        }
                        frames += 1;
                    }
                    break;
                }
            }
            for (int i = 0; i < emeraldBlocks.Count; i++)
            {
                if (collisionInfo.collider.GetInstanceID() == emeraldIdList.ElementAt(i))
                {
                if (KEActivated == false)
                {
                    blockHit.Play();
                    if (emeraldObjects.ElementAt(i).getEmeraldHitCount() == 2)
                    {
                        CameraShakerHandler.Shake(sd);
                        Instantiate(emeraldPs, emeraldBlocks.ElementAt(i).transform.position, Quaternion.identity);

                        Destroy(emeraldBlocks.ElementAt(i));
                        emeraldBlocks.Remove(emeraldBlocks.ElementAt(i));
                        emeraldObjects.Remove(emeraldObjects.ElementAt(i));
                        emeraldShakers.Remove(emeraldShakers.ElementAt(i));
                        emeraldIdList.Remove(emeraldIdList.ElementAt(i));
                        pointIncrease = 10;
                        basePoints += pointIncrease;
                    }
                    else
                    {

                        emeraldShakers.ElementAt(i).enabled = true;
                        emeraldShakers.ElementAt(i).Shake(sd2);
                        emeraldObjects.ElementAt(i).setEmeraldHitCount(emeraldObjects.ElementAt(i).getEmeraldHitCount() + 1);
                        pointIncrease = 5;
                        basePoints += pointIncrease;
                    }
                }
                else
                {
                    if (ballClones.Count > 1)
                    {
                        bonusPoints += allObjects.ElementAt(i + blocks.Count + sapphireBlocks.Count).getNetPointVal() * ballClones.Count;
                    }
                    poweredBlockHit.Play();
                    Instantiate(emeraldPs, emeraldBlocks.ElementAt(i).transform.position, Quaternion.identity);
                    
                    Destroy(emeraldBlocks.ElementAt(i));
                    emeraldBlocks.Remove(emeraldBlocks.ElementAt(i));
                    emeraldObjects.Remove(emeraldObjects.ElementAt(i));
                    emeraldShakers.Remove(emeraldShakers.ElementAt(i));
                    emeraldIdList.Remove(emeraldIdList.ElementAt(i));
                    pointIncrease = allObjects.ElementAt(i + blocks.Count+sapphireBlocks.Count).getNetPointVal() - (allObjects.ElementAt(i + blocks.Count+sapphireBlocks.Count).getHitCount() * 5);
                    
                    basePoints += pointIncrease;
                    bonusPoints += KEBonus;
                }
                    break;
                }
            }
            //Collision Detection for Ruby Blocks

            for (int i = 0; i < rubyIdList.Count; i++)
            {
                if (collisionInfo.collider.GetInstanceID() == rubyIdList.ElementAt(i) && KEActivated == false)
                {


                    
                    int frames = 0;
                    while (frames == 0)
                    {
                    
                    if (transform.position.y > (rubyBlocks.ElementAt(i).transform.position.y + (rubyBlocks.ElementAt(i).transform.localScale.y / 2)) || transform.position.y < (rubyBlocks.ElementAt(i).transform.position.y - (rubyBlocks.ElementAt(i).transform.localScale.y / 2)))
                        {

                            activeBalls.ElementAt(ballClones.IndexOf(gameObject)).setVerticalBlockCol(true);
                        }
                        else
                        {
                            activeBalls.ElementAt(ballClones.IndexOf(gameObject)).setHorizontalBlockCol(true);
                        }
                        frames += 1;
                    }
                    break;
                }
            }
            for (int i = 0; i < rubyBlocks.Count; i++)
            {
                if (collisionInfo.collider.GetInstanceID() == rubyIdList.ElementAt(i))
                {
                if (KEActivated == false)
                {
                    blockHit.Play();
                    if (rubyObjects.ElementAt(i).getRubyHitCount() == 3)
                    {
                        CameraShakerHandler.Shake(sd);
                        Instantiate(rubyPs, rubyBlocks.ElementAt(i).transform.position, Quaternion.identity);
                        Destroy(rubyBlocks.ElementAt(i));
                        rubyBlocks.Remove(rubyBlocks.ElementAt(i));
                        rubyObjects.Remove(rubyObjects.ElementAt(i));
                        rubyShakers.Remove(rubyShakers.ElementAt(i));
                        rubyIdList.Remove(rubyIdList.ElementAt(i));
                        pointIncrease = 10;
                        basePoints += pointIncrease;
                    }
                    else
                    {
                        

                        rubyShakers.ElementAt(i).enabled = true;
                        rubyShakers.ElementAt(i).Shake(sd2);
                        rubyObjects.ElementAt(i).setRubyHitCount(rubyObjects.ElementAt(i).getRubyHitCount() + 1);
                        pointIncrease = 5;
                        basePoints += pointIncrease;

                    }
                }
                else
                {
                    if (ballClones.Count > 1)
                    {
                        bonusPoints += allObjects.ElementAt(i + blocks.Count + sapphireBlocks.Count + emeraldBlocks.Count).getNetPointVal() * ballClones.Count;
                    }
                    poweredBlockHit.Play();
                    Instantiate(rubyPs, rubyBlocks.ElementAt(i).transform.position, Quaternion.identity);
                    
                    Destroy(rubyBlocks.ElementAt(i));
                    rubyBlocks.Remove(rubyBlocks.ElementAt(i));
                    rubyObjects.Remove(rubyObjects.ElementAt(i));
                    rubyShakers.Remove(rubyShakers.ElementAt(i));
                    rubyIdList.Remove(rubyIdList.ElementAt(i));
                    pointIncrease = allObjects.ElementAt(i + blocks.Count + sapphireBlocks.Count+emeraldBlocks.Count).getNetPointVal() - (allObjects.ElementAt(i + blocks.Count + sapphireBlocks.Count+emeraldBlocks.Count).getHitCount() * 5);
                    basePoints += pointIncrease;
                    bonusPoints += KEBonus;
                }
                    break;
                }
            }

            //Collision Detection for Diamond Blocks

            for (int i = 0; i < diamondIdList.Count; i++)
            {
                if (collisionInfo.collider.GetInstanceID() == diamondIdList.ElementAt(i) && KEActivated == false)
                {


                    
                    int frames = 0;
                    while (frames == 0)
                    {
                    
                    if (transform.position.y > (diamondBlocks.ElementAt(i).transform.position.y + (diamondBlocks.ElementAt(i).transform.localScale.y / 2)) || ballClones.ElementAt(ballClones.IndexOf(gameObject)).transform.position.y < (diamondBlocks.ElementAt(i).transform.position.y - (diamondBlocks.ElementAt(i).transform.localScale.y / 2)))
                        {

                            activeBalls.ElementAt(ballClones.IndexOf(gameObject)).setVerticalBlockCol(true);
                        }
                        else
                        {
                            activeBalls.ElementAt(ballClones.IndexOf(gameObject)).setHorizontalBlockCol(true);
                        }
                        frames += 1;
                    }
                    break;
                }
            }
            for (int i = 0; i < diamondBlocks.Count; i++)
            {
                if (collisionInfo.collider.GetInstanceID() == diamondIdList.ElementAt(i))
                {
                if (KEActivated == false)
                {
                    blockHit.Play();
                    if (diamondObjects.ElementAt(i).getDiamondHitCount() == 4)
                    {
                        CameraShakerHandler.Shake(sd);
                        Instantiate(diamondPs, diamondBlocks.ElementAt(i).transform.position, Quaternion.identity);

                        Destroy(diamondBlocks.ElementAt(i));
                        diamondBlocks.Remove(diamondBlocks.ElementAt(i));
                        diamondObjects.Remove(diamondObjects.ElementAt(i));
                        diamondShakers.Remove(diamondShakers.ElementAt(i));
                        diamondIdList.Remove(diamondIdList.ElementAt(i));
                        pointIncrease = 10;
                        basePoints += pointIncrease;
                    }

                    else
                    {

                        diamondShakers.ElementAt(i).enabled = true;
                        diamondShakers.ElementAt(i).Shake(sd2);
                        diamondObjects.ElementAt(i).setDiamondHitCount(diamondObjects.ElementAt(i).getDiamondHitCount() + 1);
                        pointIncrease = 5;
                        basePoints += pointIncrease;
                    }
                }
                else
                {
                    if (ballClones.Count > 1)
                    {
                        bonusPoints += allObjects.ElementAt(i + blocks.Count + sapphireBlocks.Count + emeraldBlocks.Count + rubyBlocks.Count).getNetPointVal() * ballClones.Count;
                    }
                    poweredBlockHit.Play();
                    Instantiate(diamondPs, diamondBlocks.ElementAt(i).transform.position, Quaternion.identity);
                    
                    Destroy(diamondBlocks.ElementAt(i));
                    diamondBlocks.Remove(diamondBlocks.ElementAt(i));
                    diamondObjects.Remove(diamondObjects.ElementAt(i));
                    diamondShakers.Remove(diamondShakers.ElementAt(i));
                    diamondIdList.Remove(diamondIdList.ElementAt(i));
                    pointIncrease = allObjects.ElementAt(i + blocks.Count + sapphireBlocks.Count + emeraldBlocks.Count+rubyBlocks.Count).getNetPointVal() - (allObjects.ElementAt(i + blocks.Count + sapphireBlocks.Count + emeraldBlocks.Count+rubyBlocks.Count).getHitCount() * 5);

                    basePoints += pointIncrease;
                    bonusPoints += KEBonus;
                }
                    break;
                }
            }
            for (int i = 0; i < augmentedIdList.Count; i++)
            {
                if (collisionInfo.collider.GetInstanceID() == augmentedIdList.ElementAt(i))
                {


                    allObjects.ElementAt(i).setHitCount(allObjects.ElementAt(i).getHitCount() + 1);
                    if (allObjects.ElementAt(i).getKind() == "regular" || (allObjects.ElementAt(i).getKind() == "sapphire" && allObjects.ElementAt(i).getHitCount() == 2) || (allObjects.ElementAt(i).getKind() == "emerald" && allObjects.ElementAt(i).getHitCount() == 3) || (allObjects.ElementAt(i).getKind() == "ruby" && allObjects.ElementAt(i).getHitCount() == 4) || (allObjects.ElementAt(i).getKind() == "diamond" && allObjects.ElementAt(i).getHitCount() == 5)||KEActivated==true)
                    {
                        if (allObjects.ElementAt(i).getCoinBearer() == true && allBlocks.Count > 1)
                        {
                            coinList.Add(Instantiate(goldCoin, allBlocks.ElementAt(i).transform.position, Quaternion.identity));
                            coinList.ElementAt(coinList.Count - 1).GetComponent<CoinBehaviour>().enabled = true;

                        }
                        if (allObjects.ElementAt(i).getPowerBearer() == true && allBlocks.Count > 1)
                        {
                            capsules.Add(Instantiate(powerUpCapsule, allBlocks.ElementAt(i).transform.position, Quaternion.identity));
                            powerUps.Add(new PowerUps(Random.Range(1,7)));

                        }
                        augmentedIdList.Remove(augmentedIdList.ElementAt(i));
                        allBlocks.Remove(allBlocks.ElementAt(i));
                        allObjects.Remove(allObjects.ElementAt(i));

                    }

                    break;
                }
            }
        }
        void OnCollisionExit2D(Collision2D collisionInfo)
        {

        }
    
}
