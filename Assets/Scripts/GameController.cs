using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Image timerFill;
    public float maxTime = 100;
    public float timer;

    private void Start()
    {
        timer = maxTime;   

    }
    private void Update()
    {
        UpdateTimer();
    }
    public void UpdateTimer()
    {
        timer -= Time.deltaTime;
        timerFill.fillAmount = timer / maxTime;
        if(timer <= 0)
        {
            timerFill.fillAmount = 0;
            
        }
    }
}
