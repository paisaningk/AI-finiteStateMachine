using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public TextMeshProUGUI life;
    public TextMeshProUGUI playerMode;
    public TextMeshProUGUI objectName;
    public GameObject win;
    public GameObject lose;
    public GameObject[] objectsRemain;
    public PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        objectsRemain = GameObject.FindGameObjectsWithTag("Enemy");;
        life.text = $"Life : {playerController.life}";
        playerMode.text = $"Player Mode \n: {playerController.playerMode}";
        objectName.text = $"object remain: {objectsRemain.Length}";
        
        if (objectsRemain.Length == 0)
        {
            win.SetActive(true);
            Time.timeScale = 0;
        }

        if (playerController.life == 0)
        {
            lose.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("AI_Map");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
