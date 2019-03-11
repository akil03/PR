using System.Collections;
using UnityEngine;

public class PuzzleSpawner : MonoBehaviour {
	
	public float TimeGap;
    public int direction;

    public void Spawn(GameObject puzzle, int index)
	{
        GameObject go = Instantiate(puzzle);
        go.transform.position = transform.position;
        GameManager.instance.createdPuzzel.Add(go);
        go.transform.GetChild(0).GetComponent<Puzzle>().Init();
        if (Player.instance.score > 5000 || (Player.instance.score>1000 && Player.instance.score < 1500) || (Player.instance.score>2500&&Player.instance.score<3000))
        {
            int rand = Random.Range(0,4);
            if (rand == 1)
            {
                go.transform.GetChild(0).GetComponent<Puzzle>().MoveToPlayer(direction, true);
            }
            else
            {
                go.transform.GetChild(0).GetComponent<Puzzle>().MoveToPlayer(direction);
            }
        }
        else
        {
            go.transform.GetChild(0).GetComponent<Puzzle>().MoveToPlayer(direction);
        }
        go.transform.GetChild(0).GetComponent<Puzzle>().indexofpuzzle = index;
        //go.transform.GetChild(0).GetComponent<Puzzle>().countLeft = count;
       // GameManager.instance.enemyCount += 1;
        if (!GameManager.instance.shieldActivated && !GameManager.instance.slowMotion && !GameManager.instance.godMode && !GameManager.instance.shieldCreated && !GameManager.instance.freezCreated)
        {
            int rand = Random.Range(0, 20);
            if (rand > 18 && Player.instance.score > 1500 )
            {
                ActivateFreez(go);
            }
            if(rand < 1)
            {
                ActivateShield(go);
            }
        }
	}

    public void ActivateShield(GameObject go)
    {
        //Check For Shield for firstTime for SlowMotion
        int createdShieldinGame = PlayerPrefs.GetInt("ShieldActivated", 0);
        if(createdShieldinGame == 0)
        {
            // Invoke("ActivateSlowMotion", 0.5f);
            StartCoroutine(ActivateSlowMotion("Shield !", 0.75f));
           
            //PlayerPrefs.SetInt("ShieldActivated", 1);
        }

        go.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
        go.transform.GetChild(0).GetComponent<Puzzle>().shieldActive = true;
    }

    public void ActivateFreez(GameObject go)
    {
        //Check For Freez for firstTime for SlowMotion
        int createdFreezinGame = PlayerPrefs.GetInt("FreezActivated", 0);
        if (createdFreezinGame == 0)
        {
            StartCoroutine(ActivateSlowMotion("Relax !", 0.75f));
           
            //PlayerPrefs.SetInt("FreezActivated", 1);
        }
        go.transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(true);
        go.transform.GetChild(0).GetComponent<Puzzle>().freezActivate = true;
    }

    IEnumerator ActivateSlowMotion(string text,float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 0.4f;
        Invoke("NormalSpeed", 0.5f);
        UIPop.instance.ShowText(text);
    }

    void NormalSpeed()
    {
        Time.timeScale = 1;
    }
}
