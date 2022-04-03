using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalInput : MonoBehaviour
{
    private InputActions inputActions;

    void Awake() {
        inputActions = new InputActions();
    }

    void OnEnable() {
        inputActions.Global.Enable();
        inputActions.Global.Quit.performed += ctx => Quit();
        inputActions.Global.Restart.performed += ctx => OnLevelReset();
    }

    void OnDisable() {
        inputActions.Global.Disable();
        inputActions.Global.Quit.performed -= ctx => Quit();
        inputActions.Global.Restart.performed -= ctx => OnLevelReset();
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

}
