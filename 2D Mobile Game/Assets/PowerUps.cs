using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps
{
    //Private Instance Variables
    private int powerCode;
    //Constructor
    public PowerUps(int code)
    {
        powerCode = code;
    }
    //Accessors
    public int getPowerCode()
    {
        return powerCode;
    }
    //Power-up manifestation
    
}