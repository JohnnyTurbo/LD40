using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float shotSpeed;
    public GameObject targetObject;
    public int attackStrength;

    

    void FixedUpdate() {
        MoveShot ();
    }

    public virtual void MoveShot() {

    }

    protected virtual void OnTriggerEnter2D(Collider2D col) {
        
    }
}
