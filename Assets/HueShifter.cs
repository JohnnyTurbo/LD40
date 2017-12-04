using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HueShifter : MonoBehaviour {

    public GameObject loadScreen;

    Color newColor = Color.red;
    public float timeVal = 3.0f;

    HSBColor hsbc;

	// Use this for initialization
	void Start () {
        loadScreen.SetActive (false);
        hsbc = HSBColor.FromColor (newColor);
	}
	
	// Update is called once per frame
	void Update () {
        hsbc.h = (hsbc.h + Time.deltaTime / timeVal) % 1.0f;
        Camera.main.backgroundColor = hsbc.ToColor ();
	}

    public void LoadNewScene(int sceneIndex) {
        loadScreen.SetActive (true);
        SceneManager.LoadScene (sceneIndex);
    }

    public void QuitGame() {
        Application.Quit ();
    }
}
