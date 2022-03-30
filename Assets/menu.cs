using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class menu : MonoBehaviour
{
    public static int msize = 9;
    float fade_t = 0f;
    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;
    public Image fadeRect;
    public TMP_Text number;
    void inc() { msize=Mathf.Clamp(msize+1,4,9); number.text = msize.ToString(); }
    void dec() { msize=Mathf.Clamp(msize-1,4,9); number.text = msize.ToString(); }
    void settingsMenu() { }

    void StartNewGame()
    {
        btn1.GetComponent<Button>().onClick.RemoveAllListeners();
        btn2.GetComponent<Button>().onClick.RemoveAllListeners();
        btn3.GetComponent<Button>().onClick.RemoveAllListeners();
        btn4.GetComponent<Button>().onClick.RemoveAllListeners();
        fade_t++;
    }
    void Start()
    {
        btn1.GetComponent<Button>().onClick.AddListener(StartNewGame);
        btn2.GetComponent<Button>().onClick.AddListener(dec);
        btn3.GetComponent<Button>().onClick.AddListener(inc);
        btn4.GetComponent<Button>().onClick.AddListener(Application.Quit);
    }
    void Update()
    {
        if (fade_t > 0 && fade_t < 91)
        {
            fade_t++;
            fadeRect.color = new Vector4(0f, 0f, 0f, fade_t / 90);
        }
        if (fade_t == 90f) SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
    }
}
