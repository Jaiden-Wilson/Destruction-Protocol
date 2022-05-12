using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball 
{
    // Private Instance Variables
    private int resVel;
    private float theta;
    private float ballSpeedX;
    private float ballSpeedY;
    private Vector3 ballVelX;
    private Vector3 ballVelY;
    private bool verticalBlockCol;
    private bool horizontalBlockCol;
    //Constructor
    public Ball()
    {
        resVel = 12;
        float theta = Mathf.PI / 4;
        ballSpeedX = resVel * Mathf.Cos(theta);
        ballSpeedY = resVel * Mathf.Sin(theta);
        ballVelX = new Vector3(ballSpeedX, 0, 0);
        ballVelY = new Vector3(0, ballSpeedY, 0);
        verticalBlockCol = false;
        horizontalBlockCol = false;
    }
   //Accessors
    public float getBallSpeedX()
    {
        return ballSpeedX;
    }
    public float getBallSpeedY()
    {
        return ballSpeedY;
    }
    public Vector3 getBallVelX()
    {
        return ballVelX;
    }
    public Vector3 getBallVelY()
    {
        return ballVelY;
    }
    public bool getVerticalBlockCol()
    {
        return verticalBlockCol;
    }
    public bool getHorizontalBlockCol()
    {
        return horizontalBlockCol;
    }
    //Mutators
    public void setBallSpeedX(float newBallSpeedX)
    {
        ballSpeedX= newBallSpeedX;
    }
    public void setBallSpeedY(float newBallSpeedY)
    {
        ballSpeedY = newBallSpeedY ;
    }
    public void setBallVelX( Vector3 newBallVelX)
    {
        ballVelX = newBallVelX;
    }
    public  void setBallVelY( Vector3 newBallVelY)
    {
       ballVelY = newBallVelY;
    }
    public void setVerticalBlockCol(bool newVerticalBlockCol)
    {
        verticalBlockCol=newVerticalBlockCol;
    }
    public void setHorizontalBlockCol(bool newHorizontalBlockCol)
    {
        horizontalBlockCol=newHorizontalBlockCol;
    }
}
