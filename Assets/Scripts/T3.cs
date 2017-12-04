using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T3 : Tower {

    protected override bool UseAbility() {
        Debug.Log ("Called T2 UseAbility() function");
        GameObject newProjectile = Instantiate (projectile, transform.position, Quaternion.identity);
        StartCoroutine (DestroyShot (newProjectile));
        return true;
    }

    IEnumerator DestroyShot(GameObject proj) {
        yield return new WaitForSeconds (range);
        Destroy (proj);
    }
}
