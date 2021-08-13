using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivEnemy : Enemy   
{
    
    public  override void GetShot()
    {
        if (!isClone)
        {
            isClone = true;
            Clone();
            Clone();
          
        }
        NotifyToObservers("EnemyDestroyed");
        Destroy(this.gameObject);
    }
}
