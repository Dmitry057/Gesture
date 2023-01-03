using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEditor;
public enum TypeOfGesture
{
    coldWeapon,
    dynamicGesutre
}
public enum CountHandInFunction
{
    ONE,
    TWO
}
public enum HandType
{
    LEFT,
    RIGHT
}
public enum ShowMode
{
    FIGHT,
    LOBBY,
    ALL
}
[System.Serializable]
public class Gesture : EditorWindow
{
    public string name;
    public Color color;
    public CountHandInFunction countHands;
    public ShowMode showMode;
    [Header("This components can be free")]
    public string weapoonName;
    public DynamicGesure dynamicObject;
    [Space()]
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
    public UnityEvent onDisabled;
    public TypeOfGesture typeOfGesture;
    [MenuItem("Examples/Editor GUILayout Enum Popup usage")]
    static void Init()
    {
        UnityEditor.EditorWindow window = GetWindow(typeof(HandJests));
        window.Show();
    }

    void OnGUI()
    {
        typeOfGesture = (TypeOfGesture)EditorGUILayout.EnumPopup("Primitive to create:", typeOfGesture);
        if (GUILayout.Button("Create"))
            InstantiatePrimitive(typeOfGesture);
    }

    void InstantiatePrimitive(TypeOfGesture type)
    {
        switch (type)
        {
            case TypeOfGesture.coldWeapon:

                break;
            case TypeOfGesture.dynamicGesutre:

            default:
                Debug.LogError("Unrecognized Option");
                break;
        }
    }
    public class HandJests : MonoBehaviour
    {
        // How much accurate the recognize should be
        [Header("Threshold value")]
        public float threshold = 0.1f;

        [Header("Key for save gesture")]
        public KeyCode key = KeyCode.Space;

        [Header("Hand Skeleton")]
        public OVRSkeleton skeleton;
        public Material handMaterial;
        public HandType hand;
        private GestureInputProcessor gesturePocessor;
        private SkinnedMeshRenderer mesh;

        [Header("List of Gestures")]
        public List<Gesture> gestures;

        private List<OVRBone> fingerbones = null;
        private Gesture certainGesture;


        [Header("DebugMode")]

        public bool debugMode = true;
        private bool hasStarted = false;
        private bool hasRecognize = false;
        private bool done = false;
        private bool _recogniseOtherGesture = false;

        [Header("Not Recognized Event")]
        public UnityEvent notRecognize;
        
        public void StartAction()
        {
            gesturePocessor = FindObjectOfType<GestureInputProcessor>();
            mesh = skeleton.gameObject.GetComponent<SkinnedMeshRenderer>();
            StartCoroutine(DelayRoutine(Initialize));
            handMaterial.color = Color.white;
            Debug.Log("ActionStarted");
            AddRecFunctions();
        }
        private void AddRecFunctions()
        {
            foreach (var gesture in gestures)
            {
                if (gesture.weapoonName != "")
                {
                    switch (hand)
                    {
                        case HandType.LEFT:
                            gesture.onDisabled.AddListener(gesturePocessor.DestroyLeftHandObject);
                            break;
                        case HandType.RIGHT:
                            gesture.onDisabled.AddListener(gesturePocessor.DestroyRightHandObject);
                            break;
                    }
                }
            }
        }
        public IEnumerator DelayRoutine(Action actionToDo)
        {
            yield return new WaitForSeconds(1);
            actionToDo.Invoke();
        }

        public void Initialize()
        {
            hasStarted = CheckSkeleton();
            transform.parent.GetComponent<HandObjects>().Initialise();
            switch (hand)
            {
                case HandType.LEFT:
                    gesturePocessor._leftHand = transform.parent.GetComponent<HandObjects>();
                    break;
                case HandType.RIGHT:
                    gesturePocessor._rightHand = transform.parent.GetComponent<HandObjects>();
                    break;
            }
            Debug.Log("Initialization");
        }
        public bool CheckSkeleton()
        {
            fingerbones = new List<OVRBone>(skeleton.Bones);
            return fingerbones.Count > 0;
        }
        public void Unrecognised()
        {
            handMaterial.color = Color.white;
            Debug.Log("UnRecognised");
        }
        public void UpdateAction()
        {
            if (debugMode && Input.GetKeyDown(key))
            {
                Save();
            }
            if (hasStarted && mesh.enabled)
            {
                if (!_recogniseOtherGesture)
                {
                    Debug.Log("try to reognize gesture...");
                    Gesture currentGesture = Recognize();
                    hasRecognize = !currentGesture.Equals(new Gesture());
                    if (hasRecognize)
                    {
                        done = true;
                        InvokeGesture(currentGesture);
                        currentGesture.onRecognized?.Invoke();
                        Debug.Log("Recognized!!!");
                    }
                    else
                    {
                        notRecognized();
                    }
                }
                else
                {
                    Check—ertainGesture(certainGesture, threshold + 0.03f);
                }
            }
        }
        private void notRecognized()
        {
            if (done)
            {
                done = false;
                notRecognize?.Invoke();
            }
        }
        private void InvokeGesture(Gesture gesture)
        {
            if (gesture.countHands == CountHandInFunction.ONE)
            {
                if (gesture.weapoonName != "")
                {
                    gesturePocessor.CreateObject(hand, gesture.weapoonName);
                }
            }
            else
            {
                gesturePocessor.SameFuncTwoHand(hand, gesture);
            }
            _recogniseOtherGesture = true;
            handMaterial.color = gesture.color;
            certainGesture = gesture;
        }
        private void Save()
        {
            Gesture g = new Gesture();

            g.name = "New Gesture";

            List<Vector3> data = new List<Vector3>();

            foreach (var bone in fingerbones)
            {
                data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
            }

            g.fingerDatas = data;

            gestures.Add(g);
        }

        private Gesture Recognize()
        {
            Gesture currentGesture = new Gesture();

            float currentMin = Mathf.Infinity;

            foreach (var gesture in gestures)
            {
                if (canRecognized(currentGesture.showMode))
                {
                    float sumDistance = 0;

                    bool isDiscarded = false;

                    for (int i = 0; i < fingerbones.Count; i++)
                    {
                        Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerbones[i].Transform.position);

                        float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);

                        if (distance > threshold)
                        {
                            isDiscarded = true;
                            break;
                        }
                        sumDistance += distance;
                    }

                    if (!isDiscarded && sumDistance < currentMin)
                    {
                        currentMin = sumDistance;

                        currentGesture = gesture;
                    }
                }
            }
            return currentGesture;
        }
        private void Check—ertainGesture(Gesture certainGesture, float threshold)
        {
            float currentMin = Mathf.Infinity;

            float sumDistance = 0;

            bool isDiscarded = false;

            for (int i = 0; i < fingerbones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerbones[i].Transform.position);

                float distance = Vector3.Distance(currentData, certainGesture.fingerDatas[i]);

                if (distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }
                sumDistance += distance;
            }

            if (!isDiscarded && sumDistance < currentMin)
            {
                _recogniseOtherGesture = true;
            }
            else
            {
                Debug.Log("OnDisabled");
                certainGesture.onDisabled?.Invoke();
                _recogniseOtherGesture = false;
            }

        }
        private bool canRecognized(ShowMode mode)
        {
            if (SceneManager.GetActiveScene().name == "Lobby")
            {
                return (mode == ShowMode.ALL || mode == ShowMode.LOBBY);
            }
            else
            {
                return (mode == ShowMode.ALL || mode == ShowMode.FIGHT);
            }
        }
    }
}