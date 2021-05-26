using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookPages : MonoBehaviour{
    [Header("This is where your pages go, expand as necessary")]
    public GameObject[] PagesArray;
    private int PageIndex = 0;

    public Button Prev;
    public Button Next;

    private void Update() {
        if(PageIndex == 0){
            Prev.interactable = false;
        } else {
            Prev.interactable = true;
        }

        if(PageIndex == PagesArray.Length - 1){
            Next.interactable = false;
        } else {
            Next.interactable = true;
        }
    }

    public void NextPage(){
        if(PageIndex + 1 < PagesArray.Length){
            PagesArray[PageIndex].SetActive(false);
            PageIndex += 1;
            PagesArray[PageIndex].SetActive(true);
        }
    }

    public void PreviousPage(){
        if(PageIndex - 1 >= 0){
            PagesArray[PageIndex].SetActive(false);
            PageIndex -= 1;
            PagesArray[PageIndex].SetActive(true);
        }
    }



}
