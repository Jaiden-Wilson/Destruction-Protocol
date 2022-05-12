using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FirstGearGames.SmoothCameraShaker;
using TMPro;
public class laserBehaviour : BallBehaviour
{
    //Variable Declarations and Initializations
    Rigidbody2D r;
    int laserSpeed = 30;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().enabled = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        r.MovePosition(transform.position + new Vector3(0, laserSpeed, 0) * Time.deltaTime);
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.collider.tag=="target"||collisionInfo.collider.tag=="boundary")
        {
            if (collisionInfo.collider.tag == "boundary")
            {
                Instantiate(cb, transform.position, Quaternion.identity);
                laserImpact2.Play();
            }
            else
            {
                laserImpact.Play();
            }
            Destroy(collisionInfo.otherCollider.gameObject);
            lasersLaunched = false;
            
        }
        //Collision detection for regular blocks
        for (int i = 0; i < idList.Count; i++)
        {
            if (collisionInfo.collider.GetInstanceID() == idList.ElementAt(i))
            {

                CameraShakerHandler.Shake(sd);
                blockHit.Play();
                Instantiate(ps, blocks.ElementAt(i).transform.position, Quaternion.identity);

                Destroy(blocks.ElementAt(i));
                blocks.Remove(blocks.ElementAt(i));
                idList.Remove(idList.ElementAt(i));
                pointIncrease = 10;
                basePoints += pointIncrease;
                bonusPoints += pointIncrease;
                
                break;
            }
        }

        //Collision detection for sapphire blocks
        
        for (int i = 0; i < sapphireBlocks.Count; i++)
        {
            if (collisionInfo.collider.GetInstanceID() == sapphireIdList.ElementAt(i))
            {
                if (sapphireObjects.ElementAt(i).getHitCount() == 1)
                {
                    CameraShakerHandler.Shake(sd);
                    Instantiate(sapphirePs, sapphireBlocks.ElementAt(i).transform.position, Quaternion.identity);
    
                    blockHit.Play();
                    Destroy(sapphireBlocks.ElementAt(i));
                    sapphireBlocks.Remove(sapphireBlocks.ElementAt(i));
                    sapphireObjects.Remove(sapphireObjects.ElementAt(i));
                    sapphireShakers.Remove(sapphireShakers.ElementAt(i));
                    sapphireIdList.Remove(sapphireIdList.ElementAt(i));
                    pointIncrease = 10;
                    basePoints += pointIncrease;
                    bonusPoints += pointIncrease;
                    
                }
                else
                {

                    sapphireShakers.ElementAt(i).enabled = true;
                    sapphireObjects.ElementAt(i).setHitCount(sapphireObjects.ElementAt(i).getHitCount() + 1);
                    pointIncrease = 5;
                    basePoints += pointIncrease;
                    bonusPoints += pointIncrease;

                }
                break;
            }
        }
        //Collision detection for emerald blocks
        
        for (int i = 0; i < emeraldBlocks.Count; i++)
        {
            if (collisionInfo.collider.GetInstanceID() == emeraldIdList.ElementAt(i))
            {
                if (emeraldObjects.ElementAt(i).getEmeraldHitCount() == 2)
                {
                    CameraShakerHandler.Shake(sd);
                    Instantiate(emeraldPs, emeraldBlocks.ElementAt(i).transform.position, Quaternion.identity);
    
                    blockHit.Play();
                    Destroy(emeraldBlocks.ElementAt(i));
                    emeraldBlocks.Remove(emeraldBlocks.ElementAt(i));
                    emeraldObjects.Remove(emeraldObjects.ElementAt(i));
                    emeraldShakers.Remove(emeraldShakers.ElementAt(i));
                    emeraldIdList.Remove(emeraldIdList.ElementAt(i));
                    pointIncrease = 10;
                    basePoints += pointIncrease;
                    bonusPoints += pointIncrease;
                   
                }
                else
                {

                    emeraldShakers.ElementAt(i).enabled = true;
                    emeraldShakers.ElementAt(i).Shake(sd2);
                    emeraldObjects.ElementAt(i).setEmeraldHitCount(emeraldObjects.ElementAt(i).getEmeraldHitCount() + 1);
                    pointIncrease = 5;
                    basePoints += pointIncrease;
                    bonusPoints += pointIncrease;

                }
                break;
            }
        }
        //Collision Detection for Ruby Blocks

        
        for (int i = 0; i < rubyBlocks.Count; i++)
        {
            if (collisionInfo.collider.GetInstanceID() == rubyIdList.ElementAt(i))
            {
                if (rubyObjects.ElementAt(i).getRubyHitCount() == 3)
                {
                    CameraShakerHandler.Shake(sd);
                    Instantiate(rubyPs, rubyBlocks.ElementAt(i).transform.position, Quaternion.identity);
    
                    blockHit.Play();
                    Destroy(rubyBlocks.ElementAt(i));
                    rubyBlocks.Remove(rubyBlocks.ElementAt(i));
                    rubyObjects.Remove(rubyObjects.ElementAt(i));
                    rubyShakers.Remove(rubyShakers.ElementAt(i));
                    rubyIdList.Remove(rubyIdList.ElementAt(i));
                    pointIncrease = 10;
                    basePoints += pointIncrease;
                    bonusPoints += pointIncrease;
                    
                }
                else
                {

                    rubyShakers.ElementAt(i).enabled = true;
                    rubyShakers.ElementAt(i).Shake(sd2);
                    rubyObjects.ElementAt(i).setRubyHitCount(rubyObjects.ElementAt(i).getRubyHitCount() + 1);
                    pointIncrease = 5;
                    basePoints += pointIncrease;
                    bonusPoints += pointIncrease;

                }
                break;
            }
        }

        //Collision Detection for Diamond Blocks

        for (int i = 0; i < diamondBlocks.Count; i++)
        {
            if (collisionInfo.collider.GetInstanceID() == diamondIdList.ElementAt(i))
            {
                if (diamondObjects.ElementAt(i).getDiamondHitCount() == 4)
                {
                    CameraShakerHandler.Shake(sd);
                    Instantiate(diamondPs, diamondBlocks.ElementAt(i).transform.position, Quaternion.identity);
    
                    blockHit.Play();
                    Destroy(diamondBlocks.ElementAt(i));
                    diamondBlocks.Remove(diamondBlocks.ElementAt(i));
                    diamondObjects.Remove(diamondObjects.ElementAt(i));
                    diamondShakers.Remove(diamondShakers.ElementAt(i));
                    diamondIdList.Remove(diamondIdList.ElementAt(i));
                    pointIncrease = 10;
                    basePoints += pointIncrease;
                    bonusPoints += pointIncrease;
                    
                }

                else
                {

                    diamondShakers.ElementAt(i).enabled = true;
                    diamondShakers.ElementAt(i).Shake(sd2);
                    diamondObjects.ElementAt(i).setDiamondHitCount(diamondObjects.ElementAt(i).getDiamondHitCount() + 1);
                    pointIncrease = 5;
                    basePoints += pointIncrease;
                    bonusPoints += pointIncrease;

                }
                break;
            }
        }
        for (int i = 0; i < augmentedIdList.Count; i++)
        {
            if (collisionInfo.collider.GetInstanceID() == augmentedIdList.ElementAt(i))
            {

                
                allObjects.ElementAt(i).setHitCount(allObjects.ElementAt(i).getHitCount() + 1);
                if (allObjects.ElementAt(i).getKind() == "regular" || (allObjects.ElementAt(i).getKind() == "sapphire" && allObjects.ElementAt(i).getHitCount() == 2) || (allObjects.ElementAt(i).getKind() == "emerald" && allObjects.ElementAt(i).getHitCount() == 3) || (allObjects.ElementAt(i).getKind() == "ruby" && allObjects.ElementAt(i).getHitCount() == 4) || (allObjects.ElementAt(i).getKind() == "diamond" && allObjects.ElementAt(i).getHitCount() == 5))
                {
                    if (allObjects.ElementAt(i).getCoinBearer() == true && allBlocks.Count > 1)
                    {
                        coinList.Add(Instantiate(goldCoin, allBlocks.ElementAt(i).transform.position, Quaternion.identity));
                        coinList.ElementAt(coinList.Count - 1).GetComponent<CoinBehaviour>().enabled = true;

                    }
                    if (allObjects.ElementAt(i).getPowerBearer() == true && allBlocks.Count > 1)
                    {
                        capsules.Add(Instantiate(powerUpCapsule, allBlocks.ElementAt(i).transform.position, Quaternion.identity));
                        powerUps.Add(new PowerUps(5));

                    }
                    augmentedIdList.Remove(augmentedIdList.ElementAt(i));
                    allBlocks.Remove(allBlocks.ElementAt(i));
                    allObjects.Remove(allObjects.ElementAt(i));

                }

                break;
            }
        }
    }
}
