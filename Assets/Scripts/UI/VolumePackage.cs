using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumePackage : MonoBehaviour
{
    [SerializeField]
    private string changeParameter;
    [Description("Leave empty if there's none. ")]
    public TextMeshProUGUI textMarker;
    public string ChangeParameter => changeParameter;
    public Slider Slider { private set; get; }
    void Awake()
    {
        Slider = GetComponent<Slider>();
    }
    void OnEnable()
    {
        Slider.value = PlayerSettings.GetSpecificValue(changeParameter) * 100;
        textMarker.text = Slider.value.ToString();
    }
    void Start()
    {
        Slider.value = PlayerSettings.GetSpecificValue(changeParameter) * 100;
        textMarker.text = Slider.value.ToString();
    }

}
