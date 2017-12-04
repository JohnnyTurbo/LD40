using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public int damage;
    public float range;
    public float abilityCooldown;
    public List<Enemy> enemiesInRange;
    public Material errMat, norMat;
    public GameObject projectile;
    public bool canBePlaced = true;
    public int towerCost;

    bool canFire = false;
    CircleCollider2D myCC;

    TowerState currentState = TowerState.Inactive;

    void Start() {
        myCC = GetComponent<CircleCollider2D> ();
        myCC.enabled = false;
    }

    void Update() {
        if(currentState == TowerState.Inactive) {
            transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition) - new Vector3(0,0,Camera.main.transform.position.z);
            if (Input.GetButtonDown ("Fire1")) {
                SetActive ();
            }
        }
        if (currentState == TowerState.Active && canFire && enemiesInRange.Count > 0) {
            //Debug.Log (enemiesInRange.Count + " enemies in range");
            if (UseAbility ()) {
                canFire = false;
                StartCoroutine (CooldownAbility ());
            }
        }
    }

    IEnumerator CooldownAbility() {
        //Debug.Log ("Using ability at time: " + Time.time);
        yield return new WaitForSeconds(abilityCooldown);
        canFire = true;
    }

    public void SelectTower() {
        //GetComponent<SpriteRenderer> ().material = errMat;
    }

    public void DeselectTower() {
        //GetComponent<SpriteRenderer> ().material = norMat;
    }

    protected virtual bool UseAbility() {
        Debug.LogError ("Called base class UseAbility() function");
        return false;
    }

    bool SetActive() {
        if (!canBePlaced) {
            return false;
        }
        currentState = TowerState.Active;
        canBePlaced = false;
        canFire = true;
        myCC.enabled = true;
        enemiesInRange = new List<Enemy> ();
        return true;
    }

    void OnCollisionEnter2D(Collision2D col) {
        //Debug.Log ("Entering 2D collision!");
        if (canBePlaced) {
            canBePlaced = false;
            //GetComponent<SpriteRenderer> ().material = errMat;
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if(canBePlaced == false) {
            canBePlaced = true;
            //GetComponent<SpriteRenderer> ().material = norMat;
        }
    }

    void OnCollisionStay2D(Collision2D col) {
        if (canBePlaced) {
            canBePlaced = false;
            //GetComponent<MeshRenderer> ().material = errMat;
            //GetComponent<SpriteRenderer> ().material = errMat;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        //Debug.Log ("Entering 2D Trigger with " + col.gameObject.name);
        if (currentState == TowerState.Active && col.gameObject.tag == "Enemy") {
            enemiesInRange.Add (col.gameObject.GetComponent<Enemy>());
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        //Debug.Log ("Exiting 2D Trigger with " + col.gameObject.name);
        if (currentState == TowerState.Active && col.gameObject.tag == "Enemy") {
            enemiesInRange.Remove (col.gameObject.GetComponent<Enemy>());
        }
    }
}


public enum TowerState { Active, Inactive};