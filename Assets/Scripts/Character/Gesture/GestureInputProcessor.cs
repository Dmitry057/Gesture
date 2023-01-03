using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class GestureInputProcessor : MonoBehaviour
{
   
    [Header("Hands")]
    [HideInInspector] public HandObjects _leftHand;
    [HideInInspector] public HandObjects _rightHand;

    [Header("ExampleParticles")]
    [SerializeField] private GameObject[] particles;
    [Header("Weapoons")]
    public List<Weapoon> weapoons;
    public List<GestureFunctionEvent> events;

    [Header("Links")]
    public FightUI fightUI;
    private Camera _camera;

    #region
    private GameObject ActiveObjectLeft;
    private GameObject ActiveObjectRight;
    private GameObject ActiveTwoHandObject;
    
    #endregion
    #region
    private string _memoryLeftName;
    private string _memoryRightName;
    private string _lastGestureName;
    private string _allGestureName;
    private string _publicGestureName;
    #endregion

    [SerializeField] private bool isDynamicGesture;
    private bool isDecreasedSlider;
    private bool isSameGestureAsLate(string name)
    {
        return (name == _lastGestureName);
    }

    private float _sliderLifeTime = 2;
    private float _relation;
    
    private int _comboLength = 0;

   

    private void Start()
    {
        _camera = Camera.main;
    }
    public void CreateObject(HandType hand, string nameWeapoon)
    {
        if (!isDynamicGesture)
        {

            bool found = false;
            foreach (var weapoon in weapoons)
            {
                if (nameWeapoon == weapoon._name)
                {
                    if (hand == HandType.LEFT)
                    {
                        ActiveObjectLeft = weapoon._objectLeftHand;
                        ActiveObjectLeft.SetActive(true);
                    }
                    else
                    {
                        ActiveObjectRight = weapoon._objectRightHand;
                        ActiveObjectRight.SetActive(true);
                    }

                    found = true;

                }
            }
            if (!found)
                print("Uncorrect weapoon's name!");
        }
    }
    public void SameFuncTwoHand(HandType hand, Gesture gesture)
    {
        if (hand == HandType.LEFT)
        {
            _memoryLeftName = gesture.name;
        }
        else
        {
            _memoryRightName = gesture.name;
        }
        
        if(_memoryLeftName == _memoryRightName)
        {
            if (gesture.dynamicObject != null)
            {
                if (!isDynamicGesture && !isSameGestureAsLate(gesture.name)&& _comboLength >= 0)
                {
                    Debug.Log("Try to instantiate");
                    InstantiatePointsForTwoHandDynamicGesture(gesture);
                }
            }
        }
        
    }
    private void InstantiatePointsForTwoHandDynamicGesture(Gesture gesture)
    {
        _lastGestureName = gesture.name;

        Vector3 pos = new Vector3(
        #region
            MiddlePoint(_leftHand.transform.position.x, _rightHand.transform.position.x),
            MiddlePoint(_leftHand.transform.position.y, _rightHand.transform.position.y),
            MiddlePoint(_leftHand.transform.position.z, _rightHand.transform.position.z));
        #endregion
        Vector3 targetPos = new Vector3(_camera.transform.position.x, pos.y, _camera.transform.position.z);
        ActiveTwoHandObject = Instantiate(gesture.dynamicObject.gameObject, pos,
            Quaternion.LookRotation(targetPos - pos, Vector3.up));

        DynamicGesure dynamic = ActiveTwoHandObject.GetComponent<DynamicGesure>();
        dynamic.GestureProcessor = this;

        dynamic.Left._hand = _leftHand.indexTip;
        dynamic.Right._hand = _rightHand.indexTip;
        isDynamicGesture = true;
    }
   

    public void DestroyLeftHandObject()
    {
        ActiveObjectLeft.SetActive(false);
        _memoryLeftName = "";
    }
    public void DestroyRightHandObject()
    {
        ActiveObjectRight.SetActive(false);
        _memoryRightName = "";
        
    }
    public void FinishDynamicGesture()
    {
        ShowSlider();

        Destroy(ActiveTwoHandObject);

        _allGestureName += _lastGestureName;

        isDynamicGesture = false;
    }
    private void ShowSlider()
    {
        _comboLength++;
        fightUI.ShowSlider(_comboLength);

        _relation = _sliderLifeTime;

        isDecreasedSlider = true;
    }
    private void DecreaseSlider()
    {
        if (_relation > 0)
        {
            fightUI.DecreaseSlider(_relation/_sliderLifeTime);

            if (isDynamicGesture)
            {
                fightUI.HideSlider();
                isDecreasedSlider = false;
            }
            _relation -= Time.deltaTime;
        }
        else
        {
            EndComboGesture();
            isDecreasedSlider = false;
        }
    }
    private void EndComboGesture()
    {
        OverallGestureFunction(_allGestureName);
        
        fightUI.HideSlider();
        fightUI.AddNameGestureText(_allGestureName);

        _allGestureName = "";
        _comboLength = 0;
    }
    public void TestVoidPortal(int count)
    {
        Debug.Log(count + count > 1 ? " portals" : " portal");
    }
    private void Update()
    {
        if (isDecreasedSlider)
        {
            DecreaseSlider();
        }
    }
    public void OverallGestureFunction(string name)
    {
        foreach (var _event in events)
        {
            if (_event._name == name)
            {
                _event.OnRecognize?.Invoke();
            }
        }
    }
    
    private float MiddlePoint(float start, float finish) => (start + finish) / 2;
}
[System.Serializable]
public class GestureFunctionEvent
{
    public string _name;
    public UnityEvent OnRecognize;
}

[System.Serializable]
public class GestureMemory
{
    public string _gestureName;
}

[System.Serializable]
public class Weapoon
{
    public string _name;
    public GameObject _objectLeftHand;
    public GameObject _objectRightHand;
    public float _damage;
}
