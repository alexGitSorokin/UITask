using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    #region Fields
    public delegate void ChangeOpacity(float value);
    public ChangeOpacity ChangeObjectOpacity;

    private readonly string _tubeTag = "Tube";
    private readonly string _borderTag = "BorderButtonOpacity";
    private readonly string _hideElementTag = "HideElement";
    private readonly string _selectElementTag = "SelectElement";

    private int _currentOpacityButtonIndex;

    [SerializeField] private int _opacityButtonsCount;

    [SerializeField] private float _xStartOpacityButtons;
    [SerializeField] private float _yOpacityButton;
    [SerializeField] private float _xStepOpacityButtons;

    [SerializeField] private float _xStartElementPanel;
    [SerializeField] private float _yElementPanel;
    [SerializeField] private float _xStepElementPanel;

    [SerializeField] private Toggle _hideAllToggle;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Toggle _selectAllToggle;

    private IList<Button> _opacityButtons;
    private IList<GameObject> _objectsPanel;
    private IList<Toggle> _hideToggles;
    private IList<Toggle> _selectObjectToggles;


    private readonly IList<float> _opacity = new List<float>()
        {
             1f ,
            0.80f,
            0.75f,
            0.70f,
            0.60f
        };


    [SerializeField] private Button _opacityButton;
    [SerializeField] private GameObject _elementPanel;
    #endregion

    #region Methods
    public void HideAllToggleClicked()
    {
        if (!_hideAllToggle.isOn)
            _hideToggles.ToList().ForEach(x => x.isOn = false);
        else
            _hideToggles.ToList().ForEach(x => x.isOn = true);
    }

    public void SelectAllObjectsCheckBox()
    {
        if (!_selectAllToggle.isOn)
            _selectObjectToggles.ToList().ForEach(x => x.isOn = false);
        else
            _selectObjectToggles.ToList().ForEach(x => x.isOn = true);
    }

    private void Awake()
    {
        _opacityButtons = new List<Button>();
        _objectsPanel = new List<GameObject>();
        _hideToggles = new List<Toggle>();
        _selectObjectToggles = new List<Toggle>();
    }

    public void CreateOpacityButtons()
    {
        float x = _xStartOpacityButtons;
        float y = _yOpacityButton;
        int count = _opacityButtonsCount;

        for (int i = 0; i < count; i++)
        {
            var buttonNumber = i;
            var vector = new Vector3(x, y, 0);

            var buttonObject = Instantiate(_opacityButton, vector, Quaternion.identity);

            buttonObject.transform.SetParent(transform, false);
            buttonObject.onClick.AddListener(() => OnOpacityButtonClicked(buttonNumber));
            buttonObject.onClick.AddListener(() => ChangeObjectOpacity(_opacity[buttonNumber]));

            if (i == (count - 1))
                HideButtonElement(buttonObject, _tubeTag);

            HideButtonElement(buttonObject, _borderTag);
            x += _xStepOpacityButtons;

            var images = buttonObject.GetComponentsInChildren<Image>();

            foreach (Image image in images)
            {
                var color = image.color;
                color.a = _opacity[i];
                buttonObject.GetComponent<Image>().color = color;
            }



            _opacityButtons.Add(buttonObject);
        }
    }



    private void OnOpacityButtonClicked(int buttonIndex)
    {
        HideButtonElement(_opacityButtons[_currentOpacityButtonIndex], _borderTag);
        _currentOpacityButtonIndex = buttonIndex;
        ShowButtonElement(_opacityButtons[_currentOpacityButtonIndex], _borderTag);
    }

    public GameObject CreateObjectPanel(Action onHideToggleCliked, Action onSelecteToggleCliked, string connectedElementName)
    {
        var elementsCount = _objectsPanel.Count;
 
        var vector = new Vector3(_xStartElementPanel, _yElementPanel-(elementsCount * _xStepElementPanel), 0);

        var panelElement = Instantiate(_elementPanel, vector, Quaternion.identity);
        panelElement.transform.SetParent(transform, false);

        var panelElements = panelElement.GetComponentsInChildren<Toggle>();
        var textBox = panelElement.GetComponentInChildren<Text>();

        textBox.text = connectedElementName;

        var toggleHide = panelElements.Select(x => x).Where(x=>x.tag == _hideElementTag).FirstOrDefault();
        var toggleSelect = panelElements.Select(x => x).Where(x=>x.tag == _selectElementTag).FirstOrDefault();

        toggleHide.onValueChanged.AddListener((isOn)=>onHideToggleCliked());
        toggleSelect.onValueChanged.AddListener((isOn)=> onSelecteToggleCliked());

        _objectsPanel.Add(panelElement);
        _hideToggles.Add(toggleHide);
        _selectObjectToggles.Add(toggleSelect);
        return panelElement;
    }

    private void HideButtonElement(Button button, string tag)
    {
        var images = button.GetComponentsInChildren<Image>();
        var imagesWIthTag = images.Select(x=>x).Where(x=>x.tag == tag).ToList();
        imagesWIthTag.ForEach(x => x.enabled = false);
    }

    private void ShowButtonElement(Button button, string tag)
    {
        var images = button.GetComponentsInChildren<Image>();
        var imagesWIthTag = images.Select(x => x).Where(x => x.tag == tag).ToList();
        imagesWIthTag.ForEach(x => x.enabled = true);
    }

    private void OnDisable()
    {
        _hideToggles.ToList().ForEach(x => x.onValueChanged.RemoveAllListeners());
        _selectObjectToggles.ToList().ForEach(x => x.onValueChanged.RemoveAllListeners());
        _opacityButtons.ToList().ForEach(x => x.onClick.RemoveAllListeners());
    }
    #endregion
}
