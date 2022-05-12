using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sapphire 
{
    // Private Instance Variables
    private int hitCount;
   
    //Constructor method
    public Sapphire(int hitCount)
    {
        hitCount = this.hitCount;
        
    }
    //Accessor methods
    public int getHitCount()
    {
        return hitCount;
    }
    
    //Mutator methods
    public void setHitCount(int newHitCount)
    {
        hitCount = newHitCount; 
    }
    


}
