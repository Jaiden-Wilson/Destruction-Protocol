using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class UIBehaviour : Levels
{
    //Variable Declarations
    public TextMeshProUGUI lifeCount, coinCount, score,sectionTitle, description_title, description, price;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        lifeCount.text = "" + lives;
        coinCount.text = "" + coins;
        score.text = "" + (basePoints+bonusPoints);
        price.text = "$" + cost;
        //Section Title 
        if(defenseSec==true)
        {
            sectionTitle.text = "Defense";
        }
        else if (padSec==true)
        {
            sectionTitle.text = "Paddle";
        }
        else if (ballSec == true)
        {
            sectionTitle.text = "Ball";
        }
        else if (powSec == true)
        {
            sectionTitle.text = "Power-Ups";
        }
        //Description
        if(rfClicked==true)
        {
            description_title.text = "Refill";
            description.text = "Fully restores your machine's damage tolerance ";
        }
        else if(dsClicked ==true)
        {
            description_title.text = "Double Shield";
            description.text = "Restores your machine's health by 50% and fortifies it with a secondary shield "; //75% of primary health capacity
        }
        else if (invClicked==true)
        {
            description_title.text = "Invincibility";
            description.text = "Makes your machine unsusceptible to damage for 45 ball contacts"; 
        }
        else if (u1Clicked == true)
        {
            description_title.text = "Breadmaker";
            description.text = "Doubles the value of coins collected when equipped";
        }
        else if (u2Clicked == true)
        {
            description_title.text = "Teleporter";
            description.text = "What the title says";
        }
        else if (u3Clicked == true)
        {
            description_title.text = "Annihalator";
            description.text = "Equipped with a machine gun turret that fires highly destructive rounds in short bursts";
        }
    }
    //Update and alter UI elements
    
}
