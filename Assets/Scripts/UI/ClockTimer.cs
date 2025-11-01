using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ClockTimer : MonoBehaviour
{
    public float time;
    public TextMeshProUGUI timerText;
    public Image fill;
    public float Max;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timerText.text = "" + (int)time;
        fill.fillAmount = time / Max;

        if (time < 0)
        {
            time = 0;
        }
    }

    
}
