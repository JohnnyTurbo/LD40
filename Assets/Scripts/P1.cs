using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1 : Projectile {

    void Update() {
        if (targetObject == null) {
            Destroy (gameObject);
        }
    }

    public override void MoveShot() {

        transform.position = Vector3.MoveTowards (transform.position, targetObject.transform.position, Time.deltaTime * shotSpeed);
    }

    protected override void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject == targetObject) {
            targetObject.GetComponent<Enemy> ().TakeDamage (attackStrength);
            Destroy (gameObject);
        }
    }
}
