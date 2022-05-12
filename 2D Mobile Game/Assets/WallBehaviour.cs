using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class WallBehaviour : BallBehaviour
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

                
                
                    ballClones.ElementAt(0).GetComponent<AudioSource>().Play();
                
                    activeBalls.ElementAt(i).setHorizontalBlockCol(true);
              
                    Instantiate(cb2, ballClones.ElementAt(i).transform.position, Quaternion.identity);

            }
        }
    }
}
