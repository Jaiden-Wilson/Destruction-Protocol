using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CornerBehaviour :Levels
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        for (int i = 0; i < ballClones.Count; i++)
        {
            if (collisionInfo.collider.GetInstanceID() == ballCloneIdList.ElementAt(i))
            {
                
                    theta = -Mathf.PI / 4;

                activeBalls.ElementAt(i).setBallSpeedY(resVel * Mathf.Sin(theta)); 
                activeBalls.ElementAt(i).setBallSpeedX(resVel * Mathf.Cos(theta));

            }
        }
    }
}
