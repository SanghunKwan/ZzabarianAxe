using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;
    [SerializeField] TextMeshProUGUI _characterNameText;
    [SerializeField] TextMeshProUGUI _killCountText;

    float _targetRate;



    private void Update()
    {
        float offset = _targetRate - _hpSlider.value;

        if (offset > 0)
            offset = Mathf.Min(offset, 0.01f);
        else
            offset = Mathf.Max(offset, -0.01f);

        _hpSlider.value += offset;
    }

    public void InitUI(in string characterName)
    {
        _characterNameText.text = characterName;
        SetKillCountText(0);
    }


    public void SetTargetRate(float targetRate)
    {
        _targetRate = targetRate;
    }

    public void SetKillCountText(int count)
    {
        _killCountText.text = count.ToString();
    }

}
