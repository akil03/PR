using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadscceneAsync : MonoBehaviour {

    public Image fillImage;

    private void Start()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
       
        while (!asyncLoad.isDone)
        {
            fillImage.fillAmount = asyncLoad.progress;
            yield return null;
        }
    }
}
