using UnityEngine;

public class VisiableObject : MonoBehaviour
{
    #region Fields
    private Renderer _renderer;

    private bool _isVisible = true;
    private bool _canChangeOpacity = true;
    #endregion

    #region Methods
    private void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void SetOpacityChanging()
    {
        if (_canChangeOpacity == true)
            _canChangeOpacity = false;
        else
            _canChangeOpacity = true;
    }

    public void ChangeOpacity(float opacity)
    {
        if (!_canChangeOpacity)
            return;
        SetOpacity(opacity);
    }

    public void ChangeVisibility()
    {
        if (_isVisible)
        {
            _isVisible = false;
            gameObject.SetActive(false);
        }
        else
        {
            _isVisible = true;
            gameObject.SetActive(true);
        }
    }

    private void SetOpacity(float value)
    {   print(value);
        var color = _renderer.material.color;
        color.a = value;
        _renderer.material.color = color;
    }
    #endregion
}
