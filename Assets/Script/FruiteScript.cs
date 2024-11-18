using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruiteScript : MonoBehaviour
{
    private bool isSliced = false;

    public SpriteRenderer fruitImage; // Sprite của quả cần thay đổi
    public Sprite[] fruitSprites; // Danh sách các sprite quả
    private int currentFruitIndex = 0; // Chỉ số quả hiện tại

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu dao va chạm và quả chưa bị cắt
        if (collision.CompareTag("Knife") && !isSliced)
        {
            Debug.Log("Sliced");
        }
    }

    public void ChangeFruit(int fruitIndex)
    {
        if (fruitIndex >= 0 && fruitIndex < fruitSprites.Length)
        {
            currentFruitIndex = fruitIndex;
            fruitImage.sprite = fruitSprites[currentFruitIndex];

            GameObject[] slices = GameObject.FindGameObjectsWithTag("SlicePrefab");
            foreach (GameObject slice in slices)
            {
                Destroy(slice); // Xóa SlicePrefab
            }
        }
    }
}
