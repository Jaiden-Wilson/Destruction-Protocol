using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FirstGearGames.SmoothCameraShaker;

public class ExplosionScript : MissileBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnParticleCollision(GameObject g)
    {


        // Ensure that the colliding gameObject is a block
        if (allBlocks.IndexOf(g) != -1)
        {
            Instantiate(inc, g.transform.position, Quaternion.identity);
            //incineration.Play();
            Destroy(g);

            //Point Attribution
            if (blocks.IndexOf(g) != -1)
            {
                basePoints += allObjects.ElementAt(allBlocks.IndexOf(g)).getNetPointVal();
                bonusPoints += missileBonus;
                idList.Remove(idList.ElementAt(blocks.IndexOf(g)));
                blocks.Remove(g);
            }
            else if (sapphireBlocks.IndexOf(g) != -1)
            {
                basePoints += (allObjects.ElementAt(allBlocks.IndexOf(g)).getNetPointVal() - (allObjects.ElementAt(allBlocks.IndexOf(g)).getHitCount() * 5));
                bonusPoints += missileBonus;
                sapphireIdList.Remove(sapphireIdList.ElementAt(sapphireBlocks.IndexOf(g)));
                sapphireShakers.Remove(sapphireShakers.ElementAt(sapphireBlocks.IndexOf(g)));
                sapphireBlocks.Remove(g);
            }
            else if (emeraldBlocks.IndexOf(g) != -1)
            {
                basePoints += (allObjects.ElementAt(allBlocks.IndexOf(g)).getNetPointVal() - (allObjects.ElementAt(allBlocks.IndexOf(g)).getHitCount() * 5));
                bonusPoints += missileBonus;
                emeraldIdList.Remove(emeraldIdList.ElementAt(emeraldBlocks.IndexOf(g)));
                emeraldShakers.Remove(emeraldShakers.ElementAt(emeraldBlocks.IndexOf(g)));
                emeraldBlocks.Remove(g);

            }
            else if (rubyBlocks.IndexOf(g) != -1)
            {
                basePoints += (allObjects.ElementAt(allBlocks.IndexOf(g)).getNetPointVal() - (allObjects.ElementAt(allBlocks.IndexOf(g)).getHitCount() * 5));
                bonusPoints += missileBonus;
                rubyIdList.Remove(rubyIdList.ElementAt(rubyBlocks.IndexOf(g)));
                rubyShakers.Remove(rubyShakers.ElementAt(rubyBlocks.IndexOf(g)));
                rubyBlocks.Remove(g);

            }
            else if (diamondBlocks.IndexOf(g) != -1)
            {
                basePoints += (allObjects.ElementAt(allBlocks.IndexOf(g)).getNetPointVal() - (allObjects.ElementAt(allBlocks.IndexOf(g)).getHitCount() * 5));
                bonusPoints += missileBonus;
                diamondIdList.Remove(diamondIdList.ElementAt(diamondBlocks.IndexOf(g)));
                diamondShakers.Remove(diamondShakers.ElementAt(diamondBlocks.IndexOf(g)));
                diamondBlocks.Remove(g);


            }
            //

            allObjects.Remove(allObjects.ElementAt(allBlocks.IndexOf(g)));
            augmentedIdList.Remove(augmentedIdList.ElementAt(allBlocks.IndexOf(g)));
            allBlocks.Remove(g);

        }
    }
}
