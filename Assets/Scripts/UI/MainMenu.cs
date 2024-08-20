using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject lvlSelect, mainMenu;

    public void OpenLevelSelect()
    {
        lvlSelect.SetActive(true);
        mainMenu.SetActive(false);
    
    }
    public void CloseLevelSelect()
    {
        lvlSelect.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene(2);

    }
}
