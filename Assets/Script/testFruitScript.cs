using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testFruitScript : MonoBehaviour
{
    public SpriteRenderer fruitImage;
    public Sprite[] fruitSprites;
    public int currentFruitIndex = 0;
    public void ChangeFruit(int fruitIndex)
    {
        if (fruitIndex >= 0 && fruitIndex < fruitSprites.Length)
        {
            currentFruitIndex = fruitIndex;
            fruitImage.sprite = fruitSprites[currentFruitIndex];

            GameObject[] slices = GameObject.FindGameObjectsWithTag("SlicePrefab");
            foreach (GameObject slice in slices)
            {
                Destroy(slice);
            }
        }
    }

}
