using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T1 : Tower {

    protected override bool UseAbility() {
        //Debug.Log ("Called T1 UseAbility() function");
        
        Enemy furthestEnemy = null;
        foreach (Enemy currEnemy in enemiesInRange) {
            //Debug.Log (currEnemy.name);
            if (furthestEnemy == null) {
                if (!currEnemy.isTargeted) {
                    furthestEnemy = currEnemy;
                }
            }
            else {
                if (currEnemy.GetPercentComplete () > furthestEnemy.GetPercentComplete () &&  !currEnemy.isTargeted) {
                    furthestEnemy = currEnemy;
                }
            }
        }
        if(furthestEnemy == null) {
            //Debug.Log ("could not find a target");
            return false;
        }
        GameObject newProjectile = Instantiate (projectile, transform.position, Quaternion.identity);
        newProjectile.GetComponent<Projectile> ().attackStrength = damage;
        newProjectile.GetComponent<Projectile> ().targetObject = furthestEnemy.gameObject;
        furthestEnemy.isTargeted = true;
        return true;
    }
}
