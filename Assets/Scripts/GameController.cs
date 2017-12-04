using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public float timeAlive;
    public int currentScore, totalScore, enemiesKilled, towersBuilt;
    public GameObject[] towers, enemies;
    public int[] towerCosts;
    public Button[] towerButtons;
    public Text towerNameText, towerCostText, towerDescriptionText, scoreText, gameOverText, gameOverScoreText, gameOverEnemiesText, gameOverTowersBuiltText, livesText;
    public GameObject gameOverScreen, spawnPoint;
    public float pathDistance;
    public List<GameObject> enemiesInScene, towersInScene;
    public int baseLives;
    public GameObject llStart, llnorm;

    bool isGameOver = false;
    bool isPlacingTower = false;
    Tower selectedTower = null;
    GameObject towerToBePlaced;
    Transform[] pathNodeTransforms;
    float enemySpawnInterval;
    int enemyLevel = 1;
    int currentLives;

    void Awake() {
        instance = this;
    }

    void Start() {
        currentScore = 0;
        IncreaseScore (350);
        enemiesKilled = 0;
        towersBuilt = 0;
        pathDistance = 0;
        pathNodeTransforms = GameObject.Find ("Path").GetComponentsInChildren<Transform> ();
        for(int i = 1; i < pathNodeTransforms.Length - 1; i++) {
            pathDistance += Vector3.Distance (pathNodeTransforms[i].position, pathNodeTransforms[i + 1].position);
        }
        //Debug.Log ("total distacne = " + pathDistance);
        gameOverScreen.SetActive (false);
        enemySpawnInterval = 2;
        currentLives = baseLives;
        HideTowerStats ();
        livesText.text = "Lives: " + currentLives;
        llStart.GetComponent<SpriteRenderer>().enabled = false;
        llnorm.GetComponent<SpriteRenderer> ().enabled = false;
        StartCoroutine (SpawnDelay ());
    }

    IEnumerator SpawnDelay() {
        yield return new WaitForSeconds (5f);
        StartCoroutine (SpawnEnemies ());
        
    }

    public void IncreaseScore(int amount) {
        currentScore += amount;
        scoreText.text = "Score: " + currentScore;
        for(int i = 0; i < towerCosts.Length; i++) {
            if(currentScore >= towerCosts[i]) {
                towerButtons[i].interactable = true;
            }
            else {
                towerButtons[i].interactable = false;
            }
        }
    }

    public void DecreasePlayerHealth() {
        currentLives--;
        
        if (currentLives <= 0) {
            currentLives = 0;
            //Game Over, player lost
            foreach(GameObject t in towersInScene) {
                Destroy (t);
            }
            gameOverScreen.SetActive (true);
            isGameOver = true;
            gameOverText.text = "YOU LOST!";
            gameOverEnemiesText.text = "Enemies Killed: " + enemiesKilled;
            gameOverScoreText.text = "Score: " + currentScore;
            gameOverTowersBuiltText.text = "Towers Built: " + towersBuilt;
        }

        livesText.text = "Lives: " + currentLives;
    }

    public void IncramentEnemiesKilled() {
        enemiesKilled++;
        switch (enemyLevel) {
            case 1:
                if (enemiesKilled > 5) {
                    enemyLevel++;
                    enemySpawnInterval -= 0.1f;
                    
                }
                break;
            case 2:
                if (enemiesKilled > 15) {
                    enemyLevel++;
                    enemySpawnInterval -= 0.1f;
                }
                
                break;
            case 3:
                if (enemiesKilled > 25) {
                    enemyLevel++;
                    enemySpawnInterval -= 0.1f;
                }
                break;
            case 4:
                if (enemiesKilled > 35) {
                    enemyLevel++;
                    enemySpawnInterval -= 0.1f;
                }
                break;
            case 5:
                if (enemiesKilled > 65) {
                    enemyLevel++;
                    enemySpawnInterval -= 0.25f;
                }
                break;
            case 6:
                if (enemiesKilled > 75) {
                    enemyLevel++;
                    enemySpawnInterval -= 0.25f;
                }
                break;
            case 7:
                if (enemiesKilled > 95) {
                    enemyLevel++;
                    enemySpawnInterval -= 0.25f;
                }
                break;
            case 8:
                if (enemiesKilled > 50) {
                    //enemyLevel++;
                }
                break;
            default:
                Debug.LogError ("INVALID ENEMY LEVEL");
                return;
        }
    }

    void Update() {
        if (Input.GetButtonDown ("Fire1") && !isPlacingTower) {
            //Debug.Log ("Raycasting");
            RaycastHit2D hit;
            int layerMask = LayerMask.GetMask ("SelectableTowers");
            hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, 100f, layerMask);

            if (hit.collider != null) {
                //Debug.Log ("Successful raycast on: " + hit.transform.gameObject.name);
                if (hit.transform.gameObject.tag == "Tower") {
                    //Debug.Log ("Raycasted tower");
                    if (selectedTower != null) {
                        selectedTower.DeselectTower ();
                    }
                    selectedTower = hit.transform.gameObject.GetComponent<Tower> ();
                    selectedTower.SelectTower ();
                }
                else if (selectedTower != null) {
                    //Theoretically this case should never be reached, but it is simply a precation.
                    //Debug.Log ("Deselecting tower");
                    selectedTower.DeselectTower ();
                    selectedTower = null;
                }
            }
            else if (selectedTower != null) {
                //Debug.Log ("Deselecting tower");
                selectedTower.DeselectTower ();
                selectedTower = null;
            }
        }
        else if (Input.GetButtonDown ("Fire1") && towerToBePlaced.GetComponent<Tower>().canBePlaced) {
            IncreaseScore (-1 * towerToBePlaced.GetComponent<Tower> ().towerCost);
            towersInScene.Add (towerToBePlaced);
            isPlacingTower = false;
            towerToBePlaced = null;
            towersBuilt++;
        }
        /*
        if (!isPlacingTower) {
            Debug.Log ("not placing tower at time " + Time.time);
        }
        
        if (Input.GetKeyDown (KeyCode.F2)) {
            GoLavaLamp ();
        }
        if (Input.GetKeyDown (KeyCode.F3)) {
            DecreasePlayerHealth ();
        }
        */
    }

    public void PlaceTower1() {
        ClearTowerToBePlaced ();
        isPlacingTower = true;
        towerToBePlaced = Instantiate (towers[0], new Vector3(1000, 1000, 1000), Quaternion.identity);
        GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().SetSelectedGameObject (null);
    }

    public void ShowTower1Stats() {
        towerNameText.text = "Shooter";
        towerCostText.text = "Cost 250";
        towerDescriptionText.text = "High rate of fire, precise shooting!";
    }

    public void PlaceTower2() {
        ClearTowerToBePlaced ();
        isPlacingTower = true;
        towerToBePlaced = Instantiate (towers[1], new Vector3 (1000, 1000, 1000), Quaternion.identity);
        GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().SetSelectedGameObject (null);
    }

    public void ShowTower2Stats() {
        towerNameText.text = "Star Power";
        towerCostText.text = "Cost 500";
        towerDescriptionText.text = "Shoots 5 stars in a burst!";
    }
    public void PlaceTower3() {
        ClearTowerToBePlaced ();
        isPlacingTower = true;
        towerToBePlaced = Instantiate (towers[2], new Vector3 (1000, 1000, 1000), Quaternion.identity);
        GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().SetSelectedGameObject (null);
    }

    public void ShowTower3Stats() {
        towerNameText.text = "Radio Wave";
        towerCostText.text = "Cost 750";
        towerDescriptionText.text = "Creates a radio wave burst, slowing all nearby enemies!";
    }
    public void PlaceTower4() {
        ClearTowerToBePlaced ();
        isPlacingTower = true;
        towerToBePlaced = Instantiate (towers[3], new Vector3 (1000, 1000, 1000), Quaternion.identity);
        GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().SetSelectedGameObject (null);
    }

    public void ShowTower4Stats() {
        towerNameText.text = "Cannon";
        towerCostText.text = "Cost 1000";
        towerDescriptionText.text = "Shoots powerful cannonballs!";
    }

    public void ShowTower5Stats() {
        towerNameText.text = "???";
        towerCostText.text = "Cost 2500";
        towerDescriptionText.text = "Decimate the enmeies in one fell swoop!";
    }

    void ClearTowerToBePlaced() {
        Debug.Log ("Clearing tower to be placed");
        if (isPlacingTower) {
            Debug.Log ("Clearing tower");
            Destroy (towerToBePlaced);
            towerToBePlaced = null;
        }
    }

    public void HideTowerStats() {
        towerNameText.text = "";
        towerCostText.text = "";
        towerDescriptionText.text = "Click on a tower in the panel above, then click on the playing field to place it. Cannot place towers on the path or other towers.";
    }

    public void GoLavaLamp() {
        llnorm.GetComponent<SpriteRenderer> ().enabled = true;
        llnorm.GetComponent<Animator> ().Play (0);
        IncreaseScore (-1 * towerCosts[4]);
        foreach(GameObject en in enemiesInScene) {
            if(en == null) { continue; }
            en.GetComponent<Enemy> ().speed = 0f;
            en.GetComponent<Enemy> ().rotSpeed = 0f;
        }
        isGameOver = true;

        foreach (GameObject t in towersInScene) {
            Destroy (t);
        }


        StartCoroutine ( DestroyEnemiesOnScreen ());
        StartCoroutine (ShowGameOverScreen ());
        //llnorm.GetComponent<SpriteRenderer> ().enabled = false;
    }

    IEnumerator SpawnEnemies() {
        while (!isGameOver) {
            float randSpawnInterval = Random.Range (0.25f, enemySpawnInterval);
            yield return new WaitForSeconds (randSpawnInterval);
            if (isGameOver) { break; }
            int enemyToSpawn = Random.Range (0, enemyLevel);
            GameObject newEnemy = Instantiate (enemies[enemyToSpawn], spawnPoint.transform.position, Quaternion.identity);
            enemiesInScene.Add (newEnemy);
        }
    }

    IEnumerator DestroyEnemiesOnScreen() {
        yield return new WaitForSeconds (1.25f);
        foreach(GameObject en in enemiesInScene) {
            Destroy (en);
            yield return new WaitForSeconds (.2f);
        }
    }

    IEnumerator ShowGameOverScreen() {
        yield return new WaitForSeconds (5f);
        gameOverScreen.SetActive (true);
        isGameOver = true;
        gameOverText.text = "YOU WON!";
        gameOverEnemiesText.text = "Enemies Killed: " + enemiesKilled;
        gameOverScoreText.text = "Score: " + currentScore;
        gameOverTowersBuiltText.text = "Towers Built: " + towersBuilt;

    }

    public void loadNewScene(int sceneIndex) {
        SceneManager.LoadScene (sceneIndex);
    }

}
