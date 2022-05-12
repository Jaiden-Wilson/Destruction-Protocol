using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All 
{
    //Private Instance Variables
    private bool coinBearer, powerBearer;
    private string kind;
    private int hitCount, netPointValue;
    public All(string blockType, int netPointVal)
    {
        coinBearer = false;
        powerBearer = false;
        kind = blockType;
        hitCount = 0;
        netPointValue = netPointVal;
    }
    //Accessor Methods
    public bool getCoinBearer()
    {
        return coinBearer;
    }
    public bool getPowerBearer()
    {
        return powerBearer;
    }
    public string getKind()
    {
        return kind;
    }
    public int getHitCount()
    {
        return hitCount;
    }
    public int getNetPointVal()
    {
        return netPointValue;
    }
    //Mutator Methods
    public void setCoinBearer(bool status)
    {
        coinBearer = status;
    }
    public void setHitCount( int newHitCount)
    {
        hitCount = newHitCount;
    }
    public void setPowerBearer(bool status)
    {
        powerBearer = status;
    }
}
