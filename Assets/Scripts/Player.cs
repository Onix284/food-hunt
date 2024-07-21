using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    Rigidbody2D rb;
    public float moveSpeed;
    public float rotateAmount;
    float rot;
    int score;
    public GameObject winText;
    private TrailRenderer trailRenderer;
    private bool isRotating = false;
    private bool isLevelCompleted = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLevelCompleted)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            isRotating = true;
        }
        else
        {
            isRotating = false;
        }
       
    }

    private void FixedUpdate()
    {
        if (isLevelCompleted)
        {
            rb.velocity = Vector2.zero; // Stop any ongoing movement
            return;
        }

        if (isRotating)
        {
            RotatePlayer();
        }
        MovePlayer();
    }

    void RotatePlayer()
    {
        Vector3 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

         if (mousPos.x < 0)
         {
             rot = rotateAmount;
         }
         else
         {
             rot = -rotateAmount;
         }
         transform.Rotate(0, 0, rot * Time.deltaTime);
    }


    void MovePlayer()
    {
        rb.velocity = transform.up * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "food")
        {
            Destroy(collision.gameObject);
            score++;
            if (score > 2)
                moveSpeed += 0.5f;
            if (score >= 5)
            {
                isLevelCompleted = true;
                print("Level Completed");
                winText.SetActive(true);
                StartCoroutine(GameCompletion());
            }
        }
        else if (collision.gameObject.tag == "Danger")
        {
            Destroy(trailRenderer);
            SceneManager.LoadScene("Game");
        }

        else if(collision.gameObject.tag == "Wall")
        {
            Debug.Log("Walll");
            rb.velocity = -rb.velocity;
            transform.Rotate(0, 0, 160);
            rb.velocity = transform.up * moveSpeed; 
        }

    }
    private IEnumerator GameCompletion()
    {
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }
}
