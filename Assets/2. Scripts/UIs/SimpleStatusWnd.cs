using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimpleStatusWnd : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textName;
    [SerializeField] Slider _hp;
    [SerializeField] Slider _aTimer;
    [SerializeField] float _timerShowTime;

    float _currentTimerShowedTime;


    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        if (_currentTimerShowedTime > _timerShowTime)
        {
            CloseSimpleWnd();
        }
        else
        {
            _currentTimerShowedTime += Time.deltaTime;
        }
    }
    public void CloseSimpleWnd()
    {
        gameObject.SetActive(false);


    }
    public void OpenSimpleWnd(in string name)
    {
        gameObject.SetActive(true);
        _textName.text = name;
        _hp.value = 1;
        _aTimer.value = 1;
        _currentTimerShowedTime = 0;
    }

    public void SetHPRate(float rate)
    {
        _hp.value = rate;
        gameObject.SetActive(true);
        _currentTimerShowedTime = 0;
    }
    public void SetATimer(float rate)
    {
        _aTimer.value = rate;
    }
}
