using UnityEngine;
using TMPro;

public class InfoText : MonoBehaviour
{
    TextMeshProUGUI _text;
    public static InfoText instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        _text = GetComponent<TextMeshProUGUI>();
    }
    void SetOnScreenText(string text)
    {
        _text.text = text;
    }

    public static void Static_SetOnScreenText(string text)
    {
        instance.SetOnScreenText(text);
    }
}
