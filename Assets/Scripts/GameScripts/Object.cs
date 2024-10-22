using UnityEngine;

public class Square
{
    private readonly SpriteRenderer _renderer;
    
    private bool _isActivePrev;
    private bool _isActiveCurrent;
    private int _lifeTime;
    
    private static Color _activateColor;
    private static Color _deactivateColor;
    private static Color _onTargetColor;

    private Sprite _baseSprite;
    private Sprite _happySprite;

    public Square(GameObject gameObject)
    {
        _isActivePrev = false;
        _isActiveCurrent = false;

        _activateColor = Color.white;
        _deactivateColor = Color.white;
        _onTargetColor = Color.white;

        _activateColor.a = 1f;
        _deactivateColor.a = 0f;
        _onTargetColor.a = .5f;

        _lifeTime = 0;

        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _renderer.color = _deactivateColor;
        _renderer.enabled = true;

        _baseSprite = Resources.Load<Sprite>("Base");
        _happySprite = Resources.Load<Sprite>("Happy");

        _renderer.sprite = _baseSprite;
    }

    public void Activate()
    {
        _isActiveCurrent = true;
    }

    public void Deactivate()
    {
        _lifeTime = 0;
        _isActiveCurrent = false;
        _renderer.sprite = _baseSprite; 
    }

    public void Target()
    {
        _renderer.color = _onTargetColor;
    }

    public void Untarget()
    {
        _renderer.color = _isActiveCurrent ? _activateColor : _deactivateColor;
    }

    public bool IsActive()
    {
        return _isActivePrev;
    }

    public void Update(bool real = false)
    {
        if (_isActiveCurrent && real)
        {
            _lifeTime += 1;
        }

        if (_lifeTime >= 5)
        {
            _renderer.sprite = _happySprite;
        }

        _isActivePrev = _isActiveCurrent;
        _renderer.color = _isActiveCurrent ? _activateColor : _deactivateColor;
    }
}
