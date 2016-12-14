using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static mainCharacter player;
    public static AudioPlay audioPlay;
    public static int numOfPairs = 10;
    public static Text text;
    public static Text finalText;

    public static GameObject gameOverUI;
    public static GameObject inGameUI;
    public static string textPrefix = "Pairs remaining: ";


    public static bool ended;
	// Use this for initialization
	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<mainCharacter>();
        audioPlay = GameObject.FindGameObjectWithTag("AudioPlay").GetComponent<AudioPlay>();
        text = GameObject.FindGameObjectWithTag("PairUI").GetComponent<Text>();
        gameOverUI = GameObject.FindGameObjectWithTag("GameOver");
        inGameUI = GameObject.FindGameObjectWithTag("InGame");
        Debug.Log(GameObject.FindGameObjectWithTag("FinalText"));

        finalText = GameObject.FindGameObjectWithTag("FinalText").GetComponent<Text>();
        finalText.gameObject.SetActive(false);
        text.text = textPrefix + numOfPairs;
    }
	
	// Update is called once per frame
	void Update () {
        if (ended)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }
	}

    public static void DestroyAPair()
    {
        
        numOfPairs--;
        Debug.Log("DestroyAPair: " + numOfPairs);
        if (numOfPairs >= 0)
        {
            text.text = textPrefix + numOfPairs;
        }
        else
        {
            EndGame(true);
        }
    }

    public static void Restart()
    {
        SceneManager.LoadScene("Bar2");
    }

    public static void EndGame(bool win)
    {
        gameOverUI.SetActive(true);
        inGameUI.SetActive(false);
        ended = true;
        Cursor.visible = true;
        finalText.gameObject.SetActive(true);
        if (win)
        {
            finalText.text = "You Win!";
        }
        else
        {
            finalText.text = "Game Over";
        }
    }
}
