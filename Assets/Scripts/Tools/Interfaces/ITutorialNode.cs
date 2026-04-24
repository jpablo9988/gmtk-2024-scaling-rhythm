using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public abstract class ITutorialNode : IPausable
{
    public bool IsActive { private set; get; }
    [Header("Dependants")]
    [SerializeField]
    protected ITutorialNode[] dependantNodes;
    [SerializeField]
    private bool setChildrenNodesAsDependants;
    [HideInInspector]
    public bool isDependant = false;
    [Header("Custom Activations")]
    [SerializeField]
    protected CustomActivators[] activateOnExecute;
    [Serializable]
    public struct CustomActivators
    {
        public MonoBehaviour mb;
        public GameObject go;
        public UnityEvent executeOnStart;
        public UnityEvent executeOnEnd;
        public bool deactivateOnCleanup;
    }
    private Action onEndExecute = null;
    void Awake()
    {
        if (setChildrenNodesAsDependants)
        {
            dependantNodes = GetComponentsInChildren<ITutorialNode>().Where(i => i != this).ToArray();
        }
        foreach (ITutorialNode node in dependantNodes)
        {
            node.isDependant = true;
        }
    }

    protected T GetDependant<T>(T dependant) where T : UnityEngine.Object
    {
        if (dependant == null)
        {
            Debug.LogWarning(typeof(T).Name + " not assigned for " + gameObject.name + ". Searching one in scene...");
            return FindFirstObjectByType<T>();
        }
        else return dependant;
    }
    void Start()
    {
        IsActive = false;
    }
    public virtual void ExecuteNode(Action onEndExecute)
    {
        IsActive = true;
        foreach (CustomActivators ca in activateOnExecute)
        {
            if (ca.mb != null) ca.mb.enabled = true;
            if (ca.go != null) ca.go.SetActive(true);
            if (ca.executeOnStart != null) ca.executeOnStart?.Invoke();
        }
        foreach (ITutorialNode nodes in dependantNodes)
        {
            nodes.ExecuteNode(() => { });
        }
        this.onEndExecute = onEndExecute;
    }
    public virtual void CleanNode()
    {
        IsActive = false;
        foreach (CustomActivators ca in activateOnExecute)
        {
            if (ca.deactivateOnCleanup)
            {
                if (ca.mb != null) ca.mb.enabled = false;
                if (ca.go != null) ca.go.SetActive(false);
            }
            if (ca.executeOnEnd != null) ca.executeOnEnd?.Invoke();
        }
        foreach (ITutorialNode nodes in dependantNodes)
        {
            if (nodes.IsActive) nodes.CleanNode();
        }
        onEndExecute?.Invoke();
    }
}
