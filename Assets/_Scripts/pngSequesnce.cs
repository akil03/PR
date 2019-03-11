using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pngSequesnce : MonoBehaviour {

	public Sprite[] images;
    public SpriteRenderer spritetobechanged;
    public float fps;
    public bool isImage;
    public Image imageTobeChanged;

	// Use this for initialization
	IEnumerator Start () {
		foreach(Sprite img in images) {
            if(!isImage)
			  spritetobechanged.sprite = img;
            else
                imageTobeChanged.sprite = img;
            yield return new WaitForSeconds(1/fps);
		}

        yield return new WaitForSeconds(Random.Range(2, 7));
		StartCoroutine (Start ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
