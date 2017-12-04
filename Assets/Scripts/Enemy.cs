using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int baseHealth, scoreValue;
    public float speed, rotSpeed;
    public bool immuneToEffects;
    public bool isTargeted;

    Transform[] thePath;
    int pathNodeIndex;
    int curHealth;
    float distanceTraveled = 0;
    float toatlDistance;
    Vector3 posLastFrame;
    float percentComplete = 0;

    void Start() {
        thePath = GameObject.Find ("Path").GetComponentsInChildren<Transform> ();
        pathNodeIndex = 1;
        curHealth = baseHealth;
        posLastFrame = transform.position;
        toatlDistance = GameController.instance.pathDistance;
    }

    void Update() {
        distanceTraveled += Vector3.Distance (posLastFrame, transform.position);
        posLastFrame = transform.position;
    }

    void FixedUpdate() {
        if(Vector3.Distance(transform.position, thePath[pathNodeIndex].position) <= 0.001f) {
            pathNodeIndex++;
            if (pathNodeIndex >= thePath.Length) {
                ReachedFinalNode ();
                return;
            }
        }
        transform.position = Vector3.MoveTowards (transform.position, thePath[pathNodeIndex].position, Time.deltaTime * speed);
        Vector3 fromEnemyToNextPos = thePath[pathNodeIndex].position - transform.position;
        float rotToNextPos = Vector3.Angle (Vector3.down, fromEnemyToNextPos);
        rotToNextPos = (fromEnemyToNextPos.x >= 0) ? rotToNextPos : rotToNextPos * -1;
        Quaternion targetRotation = Quaternion.Euler (0, 0, rotToNextPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
    }

    void ReachedFinalNode() {
        GameController.instance.DecreasePlayerHealth ();
        Destroy (gameObject);
    }

    public float GetPercentComplete() {
        percentComplete = distanceTraveled / toatlDistance;
        //Debug.Log (gameObject.name + " has traveled " + distanceTraveled + " of a total " + toatlDistance + " making it " + percentComplete + " percent complete");
        return percentComplete;
    }

    public void TakeDamage(int damageAmount) {
        //Debug.Log ("Taking damage value: " + damageAmount);
        curHealth -= damageAmount;
        if(curHealth <= 0) {
            GameController.instance.IncreaseScore (scoreValue);
            GameController.instance.enemiesInScene.Remove (gameObject);
            GameController.instance.IncramentEnemiesKilled ();
            Destroy (this.gameObject);
        }
        isTargeted = false;
    }
}
