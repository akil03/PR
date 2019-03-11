using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFillTime : MonoBehaviour 
{
    [SerializeField]
    float speed;

    [SerializeField]
    Image fillImage;

    [SerializeField]
    float delayTiming;

    [SerializeField]
    GameObject nothanksButton, retrybutton;

    bool animate;
	// Use this for initialization
	void Start () 
    {
        Invoke("Delay", delayTiming);
	}
	
	// Update is called once per frame
	void Update () 
    {
      //  print(fillImage.fillAmount > 0.5f);
        if(animate && fillImage.fillAmount<1)
        {
            fillImage.fillAmount += speed * Time.deltaTime;
        }
        if(fillImage.fillAmount >=1)
        {
            retrybutton.SetActive(true);
            nothanksButton.SetActive(false);
            gameObject.SetActive(false);
        }
        if (fillImage.fillAmount > 0.5f && fillImage.fillAmount <1f)
        {
            nothanksButton.SetActive(true);
            nothanksButton.transform.localScale = Vector3.one;
        }
	}

    void Delay()
    {
        animate = true;
    }
}
