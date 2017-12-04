using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2 : Projectile {
    protected override void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Enemy") {
            col.GetComponent<Enemy> ().TakeDamage (attackStrength);
            Destroy (gameObject);
        }
    }

    public override void MoveShot() {

        //transform.position = Vector3.MoveTowards (transform.position, transform.localPosition * 5f, Time.deltaTime * shotSpeed);
        transform.Translate (Vector3.up * Time.deltaTime * shotSpeed);
    }
}
