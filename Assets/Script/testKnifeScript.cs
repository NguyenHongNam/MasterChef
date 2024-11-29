using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class testKnifeScript : MonoBehaviour
{
    public float speed = 0.3f;
    public float verticalDistance = 1f;
    public float verticalSpeed = 5f;
    private bool isMovingVertically = false;
    private Vector3 originalPosition;

    public GameObject cutMarkPrefab;
    public Transform fruitTransform;

    public GameObject WinPanel;
    public GameObject StartPanel;
    public UnityEngine.UI.Button restartButton;
    public UnityEngine.UI.Button nextButton;

    public bool isGameStart;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        originalPosition = transform.position;
        StartPanel.SetActive(true);
        WinPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (Input.touchCount > 0 && !isMovingVertically && isGameStart) 
        {
            Touch touch = Input.GetTouch(0); 

            if (touch.phase == TouchPhase.Began)
            {
                StartCoroutine(MoveVertical());
            }
        }
    }

    private System.Collections.IEnumerator MoveVertical()
    {
        isMovingVertically = true;
        Vector3 startPosition = transform.position;

        Vector3 targetDown = startPosition + Vector3.down * verticalDistance;
        while (Vector3.Distance(transform.position, targetDown) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetDown, verticalSpeed * Time.deltaTime);
            yield return null;
        }

        Vector3 targetUp = startPosition;
        while (Vector3.Distance(transform.position, targetUp) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetUp, verticalSpeed * Time.deltaTime);
            yield return null;
        }

        isMovingVertically = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("StopKnife"))
        {
            isGameStart = false;
            Time.timeScale = 0;
            WinPanel.SetActive(true);
            transform.position = originalPosition;
        }else if (collision.gameObject.tag == "StopCut")
        {
            isGameStart = false;
        }
        else if (collision.gameObject.tag == "Fruit")
        {
            Debug.Log("Cut");
            CreateCutMark();
        }
    }

    private void CreateCutMark()
    {
        if (cutMarkPrefab != null && fruitTransform != null)
        {
            Vector3 knifePosition = transform.position;

            Vector3 cutPosition = new Vector3(knifePosition.x, fruitTransform.position.y, fruitTransform.position.z);
            Instantiate(cutMarkPrefab, cutPosition, Quaternion.identity, fruitTransform);
        }
    }

    public void RestartGame()
    {
        isGameStart=true;
        GameObject[] slices = GameObject.FindGameObjectsWithTag("SlicePrefab");
        foreach (GameObject slice in slices)
        {
            Destroy(slice);
        }
        Time.timeScale = 1;
        WinPanel.SetActive(false);
    }

    public void NextFruit()
    {
        isGameStart = true;
        testFruitScript fruitScript = fruitTransform.GetComponent<testFruitScript>();
        if (fruitScript != null)
        {
            fruitScript.ChangeFruit((fruitScript.currentFruitIndex + 1) % fruitScript.fruitSprites.Length);
        }
        GameObject[] slices = GameObject.FindGameObjectsWithTag("SlicePrefab");
        foreach (GameObject slice in slices)
        {
            Destroy(slice);
        }
        Time.timeScale = 1;
        WinPanel.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        StartPanel.SetActive(false);
        isGameStart=true;
    }

}
