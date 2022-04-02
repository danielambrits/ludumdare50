using UnityEngine;
using TMPro;

public class OvertextHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ui;

    public void SetValue(int value) {
        if (value == 0) {
            ui.text = "";
        } else {
            ui.text = value.ToString();
        }
    }
}
