using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T2 : Tower {

    protected override bool UseAbility() {
        Debug.Log ("using p2 ability");
        for(int i = 0; i < 5; i++) {
            //Debug.Log ("for loop");
            Quaternion targetRot = Quaternion.Euler (0, 0, i * 72 + 18);
            GameObject newProjectile = Instantiate (projectile, transform.position, targetRot);
            StartCoroutine( DestroyShot (newProjectile));
        }
        return true;
    }

    IEnumerator DestroyShot(GameObject proj) {
        Debug.Log ("starting DS");
        yield return new WaitForSeconds (range);
        Destroy (proj);
        Debug.Log ("ending DS");
    }
}
