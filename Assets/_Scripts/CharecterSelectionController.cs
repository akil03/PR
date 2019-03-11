using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharecterSelectionController : MonoBehaviour {


    public static CharecterSelectionController instance;

    public GameObject leftButton, rightButton;

    Animator leftAnimator, rightAnimator;

    private void Awake()
    {
        instance = this;
    }


    void Update () 
    {
        if(GameManager.instance.indexOftheCurrentCharecter == 0 && leftButton)
        {
            if (leftButton.activeSelf)
            {

                leftButton.SetActive(false);
            }

        }
        if(GameManager.instance.indexOftheCurrentCharecter == GameManager.instance.charecterSelection.Count -1  && rightButton)
        {
            if (rightButton.activeSelf)
            {
                rightButton.SetActive(false);
            }
        }

        if(GameManager.instance.indexOftheCurrentCharecter >0 && GameManager.instance.indexOftheCurrentCharecter < GameManager.instance.charecterSelection.Count - 1 && leftButton && rightButton)
        {
            if(!leftButton.activeSelf)
            {
                leftButton.SetActive(true);
            }
            if(!rightButton.activeSelf)
            {
                rightButton.SetActive(true);
            }

        }


	}
}
