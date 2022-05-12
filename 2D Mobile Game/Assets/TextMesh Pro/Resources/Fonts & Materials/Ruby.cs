using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruby 
{
    // Private Instance Variables
    private int hitCount;
    //Constructor method
    public Ruby(int hitCount)
    {
        hitCount = this.hitCount;
    }
    //Accessor method
    public int getRubyHitCount()
    {
        return hitCount;
    }
    //Mutator method
    public void setRubyHitCount(int newHitCount)
    {
        hitCount = newHitCount;
    }
}
