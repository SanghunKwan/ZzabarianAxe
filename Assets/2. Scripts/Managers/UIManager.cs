using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;

    float _targetRate;



    private void Update()
    {
        float offset = _targetRate - _hpSlider.value;

        if (offset > 0)
            offset = Mathf.Min(offset, 0.05f);
        else
            offset = Mathf.Max(offset, -0.05f);

        _hpSlider.value += offset;
    }


    public void SetTargetRate(float targetRate)
    {
        _targetRate = targetRate;
    }



}
