using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalInput : MonoBehaviour
{
    [SerializeField]
    private GameObject helpUi;

    private InputActions inputActions;

    void Awake() {
        inputActions = new InputActions();
    }

    void OnEnable() {
        inputActions.Global.Enable();
        inputActions.Global.Quit.performed += ctx => Quit();
        inputActions.Global.Restart.performed += ctx => OnLevelReset();
        inputActions.Global.ShowHelp.performed += ctx => OnShowHelpUi();
        inputActions.Global.HideHelp.performed += ctx => OnHideHelpUi();
    }

    void OnDisable() {
        inputActions.Global.Disable();
        inputActions.Global.Quit.performed -= ctx => Quit();
        inputActions.Global.Restart.performed -= ctx => OnLevelReset();
        inputActions.Global.ShowHelp.performed -= ctx => OnShowHelpUi();
        inputActions.Global.HideHelp.performed -= ctx => OnHideHelpUi();
    }

    private void OnLevelReset() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Quit() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void OnShowHelpUi() {
        helpUi.SetActive(true);
        inputActions.Global.HideHelp.performed += ctx => OnHideHelpUi();
    }

    private void OnHideHelpUi() {
        helpUi.SetActive(false);
        inputActions.Global.HideHelp.performed -= ctx => OnHideHelpUi();
    }

}
