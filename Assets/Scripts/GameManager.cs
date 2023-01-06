using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields
    private int _objectCount = 5;
    [SerializeField] private GameObject _cubePrefub;
    [SerializeField] private GameObject _spherePrefub;

    private UIPanel _ui;

    readonly IList<VisiableObject> _sceneObjects = new List<VisiableObject>();
    #endregion

    #region Properties
    private Vector3 RandomVector
    {
        get => new Vector3(Random.Range(-3, 25), Random.Range(1, 15), Random.Range(15, 50));
    }
    #endregion

    #region Methods
    private void Awake()
    {
        _ui = FindObjectOfType<UIPanel>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    void Start()
    {
        SpawnObjects();
    }

    public void SpawnObjects()
    {
        _ui.CreateOpacityButtons();

        for (int i = 0; i < _objectCount; i++)
        {
            var sceneObject = Instantiate(_cubePrefub, RandomVector, Quaternion.identity);

            sceneObject.name = $"Element {i}";

            var visibleObject = sceneObject.GetComponent<VisiableObject>();
            _ui.ChangeObjectOpacity += visibleObject.ChangeOpacity;
            var elementPanel = _ui.CreateObjectPanel(visibleObject.ChangeVisibility, 
            visibleObject.SetOpacityChanging, sceneObject.name);

            _sceneObjects.Add(visibleObject);
        }
    }

    private void OnDisable()
    {
        _sceneObjects.ToList().ForEach(x => _ui.ChangeObjectOpacity -= x.ChangeOpacity);
    }
    #endregion
}
