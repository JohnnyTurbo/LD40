using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3 : Projectile {

    protected override void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Enemy") {
            if (!col.gameObject.GetComponent<Enemy> ().immuneToEffects) {
                col.GetComponent<Enemy> ().speed = col.GetComponent<Enemy> ().speed * 0.5f;
                col.GetComponent<Enemy> ().immuneToEffects = true;
            }
        }
    }

    public override void MoveShot() {

        transform.localScale = Vector3.Lerp (transform.localScale, Vector3.one * 5, Time.deltaTime * shotSpeed);
    }
}
