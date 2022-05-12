using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emerald 
{
    // Private Instance Variables
    private int hitCount;
    //Constructor method
    public Emerald(int hitCount)
    {
        hitCount = this.hitCount;
    }
    //Accessor method
    public int getEmeraldHitCount()
    {
        return hitCount;
    }
    //Mutator method
    public void setEmeraldHitCount(int newHitCount)
    {
        hitCount = newHitCount;
    }
}
