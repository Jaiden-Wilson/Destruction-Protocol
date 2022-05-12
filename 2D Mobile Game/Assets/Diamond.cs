using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond
{
    // Private Instance Variables
    private int hitCount;
    //Constructor method
    public Diamond(int hitCount)
    {
        hitCount = this.hitCount;
    }
    //Accessor method
    public int getDiamondHitCount()
    {
        return hitCount;
    }
    //Mutator method
    public void setDiamondHitCount(int newHitCount)
    {
        hitCount = newHitCount;
    }
}
