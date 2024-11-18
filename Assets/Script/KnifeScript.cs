using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    public float speed = 0.2f;
    public bool isMoving;

    public float downDistance = 2f;   // Khoảng cách di chuyển xuống
    public float downTime = 0.05f;       // Thời gian di chuyển xuống
    public float upTime = 0.001f;
    public Vector3 lastPosition;


    public GameObject cutMarkPrefab;  // Prefab vết cắt
    public Transform fruitTransform;

    public float cooldownTime = 0.2f;
    public bool canCut;

    public GameObject pn;

    public Vector2 originalPos;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving){
            Vector3 newPosition = transform.position;
            newPosition.x += speed * Time.deltaTime;
            transform.position = newPosition;
        }
        if (canCut && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            lastPosition = transform.position;
            StartCoroutine(MoveDownAndUp());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SliceZone")
        {
            Time.timeScale = 0;
            isMoving = false;
            canCut = false;
            pn.SetActive(true);
        }
        else if (collision.gameObject.tag == "Fruit") // Dao chạm quả
        {
            CreateCutMark();
        }
    }
    private IEnumerator MoveDownAndUp()
    {
        canCut = false;
        Vector3 downPosition = new Vector3(lastPosition.x, lastPosition.y - downDistance, lastPosition.z);

        // Di chuyển xuống
        float elapsedTime = 0;
        while (elapsedTime < downTime)
        {
            transform.position = Vector3.Lerp(lastPosition, downPosition, (elapsedTime / downTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = downPosition;

        // Di chuyển lên lại về vị trí đã lưu
        elapsedTime = 0;
        while (elapsedTime < upTime)
        {
            transform.position = Vector3.Lerp(downPosition, lastPosition, (elapsedTime / upTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = lastPosition;
        yield return new WaitForSeconds(cooldownTime);
        canCut = true;
    }

    private void CreateCutMark()
    {
        if (cutMarkPrefab != null && fruitTransform != null)
        {
            // Vị trí của dao trên quả
            Vector3 knifePosition = transform.position;

            // Sinh ra vết cắt tại vị trí tương ứng trên quả
            Vector3 cutPosition = new Vector3(knifePosition.x, fruitTransform.position.y, fruitTransform.position.z);
            Instantiate(cutMarkPrefab, cutPosition, Quaternion.identity, fruitTransform);
        }
    }
}
