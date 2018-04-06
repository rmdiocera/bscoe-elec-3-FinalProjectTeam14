using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController2 : MonoBehaviour {

    [SerializeField] float reloadDelay = 1f;
    [SerializeField] int scoreHit = 50;

    private bool isGrounded;

    Rigidbody rb;

    ScoreBoard scoreBoard;

	// Use this for initialization
	void Start () {

        scoreBoard = FindObjectOfType<ScoreBoard>();
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 30.0f;

        rb.transform.Rotate(0, x, 0);
        rb.transform.Translate(0, 0, z);

        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(0, 20, 0);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        switch (collision.gameObject.tag)
        {

            case "Terrain":
                isGrounded = true;
                break;

            case "Crate":
                Invoke("NextLevel", reloadDelay);
                break;

            default:
                print("hoop collision");
                Invoke("RestartLevel", reloadDelay);
                break;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Star":
                FruitCollision();
                print("star collision");
                Destroy(other.transform.gameObject);
                break;

        }
    }

    private void FruitCollision()
    {
        scoreBoard.ScoreHit(scoreHit);
    }

    private void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 1;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

}
