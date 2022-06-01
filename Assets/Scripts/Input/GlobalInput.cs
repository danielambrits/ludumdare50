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
        inputActions.Global.ToggleHelp.performed += ctx => OnShowHelpUi(!helpUi.active);
    }

    void OnDisable() {
        inputActions.Global.Disable();
        inputActions.Global.Quit.performed -= ctx => Quit();
        inputActions.Global.Restart.performed -= ctx => OnLevelReset();
        inputActions.Global.ToggleHelp.performed -= ctx => OnShowHelpUi(!helpUi.active);
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

    private void OnShowHelpUi(bool show) {
        helpUi.SetActive(show);
    }

}
