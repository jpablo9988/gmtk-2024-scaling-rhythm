using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject lvlSelect, mainMenu, settingsMenu, creditsMenu, quitButton;

    private bool isSettingsMenuOpen = false;
    private bool isCreditsMenuOpen = false;
    void OnEnable()
    {
        isSettingsMenuOpen = false;
        isCreditsMenuOpen = false;
#if UNITY_WEBGL
        quitButton.SetActive(false);
#endif
    }
    public void OpenLevelSelect()
    {
        lvlSelect.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void ToggleSettingsMenu()
    {
        isSettingsMenuOpen = !isSettingsMenuOpen;
        settingsMenu.SetActive(isSettingsMenuOpen);
        mainMenu.SetActive(!isSettingsMenuOpen);
    }
    public void ToggleCreditsMenu()
    {
        isCreditsMenuOpen = !isCreditsMenuOpen;
        creditsMenu.SetActive(isCreditsMenuOpen);
        mainMenu.SetActive(!isCreditsMenuOpen);
    }
    public void CloseLevelSelect()
    {
        lvlSelect.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene((int)Scenes.MICROBE_MUNCHIN);
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene((int)Scenes.ROCK_TOSSER);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
