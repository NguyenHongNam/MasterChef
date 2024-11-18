using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel;      // Panel xuất hiện khi dao chạm SliceZone
    public GameObject startPanel;      // Panel xuất hiện khi bắt đầu game
    public Text countdownText;         // Text hiển thị đếm ngược
    public KnifeScript knifeScript;    // Tham chiếu đến script dao
    public FruiteScript fruitScript;   // Tham chiếu đến script quả
    public Sprite[] fruitSprites;      // Danh sách các sprite quả

    //private bool gameStarted = false;

    private int countDown = 5;
    private bool isGameStart;

    private int currentFruitIndex = 0; // Chỉ số của loại quả hiện tại

    private void Start()
    {
        Debug.Log("Qua hien tai: " +currentFruitIndex);
        startPanel.SetActive(true);
        pausePanel.SetActive(false);
        Time.timeScale = 0;
    }

    private void Update()
    {
        if(!isGameStart && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))){
            Debug.Log("Start game");
            StartCoroutine(StartCountdown());
        }
    }

    //private IEnumerator StartCountdown()
    //{
    //    gameStarted = true; // Đánh dấu game đang bắt đầu
    //    yield return new WaitForSeconds(5f); // Chờ 5 giây
    //    startPanel.SetActive(false);
    //    Time.timeScale = 1; // Bắt đầu game sau 5 giây
    //    knifeScript.isMoving = true; // Cho phép dao di chuyển
    //}

    private IEnumerator StartCountdown()
    {
        Time.timeScale = 1; // Kích hoạt thời gian game để đếm ngược hoạt động

        while (countDown > 0)
        {
            if (countdownText != null)
                countdownText.text = countDown.ToString(); // Hiển thị số giây còn lại
            yield return new WaitForSeconds(1f);
            countDown--;
        }

        if (countdownText != null)
            countdownText.text = ""; // Xóa text đếm ngược

        startPanel.SetActive(false);
        isGameStart = true;
        knifeScript.isMoving = true;
        knifeScript.canCut = true;
    }
    public void RestartGame()
    {
        pausePanel.SetActive(false);
        startPanel.SetActive(true);
        isGameStart = false;
        countDown = 5;
        knifeScript.transform.position = knifeScript.originalPos;
        fruitScript.ChangeFruit(currentFruitIndex);
    }

    public void NextLevel()
    {
        knifeScript.transform.position = knifeScript.originalPos;
        currentFruitIndex = (currentFruitIndex + 1) % fruitSprites.Length;
        fruitScript.ChangeFruit(currentFruitIndex);
        pausePanel.SetActive(false);
        startPanel.SetActive(true);
        isGameStart = false;
        countDown = 5;
    }

}
