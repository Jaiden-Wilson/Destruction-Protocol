using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PaddingScript : Levels
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
                if (healthActivated == true)
                {
                    health -= 1;
                    healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x - originalHealthBarScale / dmgTol, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
                    healthBar.transform.localPosition = new Vector3(healthBar.transform.localPosition.x - 0.1183f, healthBar.transform.localPosition.y, healthBar.transform.localPosition.z);
                }
                if (transform.position.x < ballClones.ElementAt(i).transform.position.x)
                {
                    theta = Mathf.PI / 2 - ((ballClones.ElementAt(i).transform.position.x - transform.position.x) * Mathf.PI / 6);
                }
                else if (transform.position.x > ballClones.ElementAt(i).transform.position.x)
                {
                    theta = Mathf.PI / 2 + ((transform.position.x - ballClones.ElementAt(i).transform.position.x) * Mathf.PI / 6);
                }
                activeBalls.ElementAt(i).setBallSpeedX(resVel * Mathf.Cos(theta));
                activeBalls.ElementAt(i).setBallSpeedY(resVel * Mathf.Sin(theta));
            }
        }
    }
}
