using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIPortaitHandler : MonoBehaviour
{
    [Serializable]
    public struct IdleStates
    {
        [Tooltip("Name of the Trigger in the animator.")]
        public string _triggerName;
        [Tooltip("Min. amount of time this animation will trigger.")]
        public float _minRange;
        [Tooltip("Max. amount of time this animation will trigger. If lesser or equal than the minimum, it will always choose this value.")]
        public float _maxRange;
    }
    [SerializeField]
    private List<IdleStates> _sharedIdleStates = new();
    [SerializeField]
    private string _defaultPortraitName = "default";
    [SerializeField]
    private string _talkingPortraitName = "Talk";
    private Animator a_portait;
    private float _currentIdleTimer = 0.0f;
    private int _currentIdleIndex = 0;
    private List<IdleStates> _idleStates = new();
    public bool IsIdle;

    void Awake()
    {
        a_portait = GetComponent<Animator>();
    }
    void OnEnable()
    {
        IsIdle = false;
        _currentIdleTimer = -1.0f;
        _idleStates = new(_sharedIdleStates);
    }
    public void SetDialoguePortrait(string stateName, int layer = 0)
    {
        int stateID = Animator.StringToHash(stateName);
        if (a_portait.HasState(layer, stateID))
        {
            a_portait.Play(stateID);
            IsIdle = true;
        }
        else
        {
            Debug.LogWarning("Using this tag: " + stateName + ", We couldn't find the portrait.");
            a_portait.Play(_defaultPortraitName);
        }
    }
    public void SetTalkingAnimation(bool isTalking)
    {
        a_portait.SetBool(_talkingPortraitName, isTalking);
        IsIdle = !isTalking;
    }
    public void SetDialoguePortrait(int stateHash, int layer = 0)
    {
        if (a_portait.HasState(layer, stateHash))
            a_portait.Play(stateHash);
        else
        {
            Debug.LogWarning("Using this tag: " + stateHash + ", We couldn't find the portrait.");
            a_portait.Play(_defaultPortraitName);
        }
    }
    private void Update()
    {
        if (_idleStates.Count == 0 || !IsIdle) return;
        if (_currentIdleTimer <= 0.0f)
        {
            if (_currentIdleTimer > -1.0f)
            {
                this.a_portait.SetTrigger(_idleStates[_currentIdleIndex]._triggerName);
            }
            _currentIdleIndex = UnityEngine.Random.Range(0, _idleStates.Count);
            if (_idleStates[_currentIdleIndex]._minRange >= _idleStates[_currentIdleIndex]._maxRange)
            {
                _currentIdleTimer = _idleStates[_currentIdleIndex]._maxRange;
            }
            else
            {
                _currentIdleTimer = UnityEngine.Random.Range(_idleStates[_currentIdleIndex]._minRange
                , _idleStates[_currentIdleIndex]._maxRange);
            }
        }
        _currentIdleTimer -= Time.deltaTime;

    }
    public void AddIdleStates(List<IdleStates> newIdleStates, bool includeSharedStates = true)
    {
        if (!includeSharedStates) _idleStates.Clear();
        _idleStates.AddRange(newIdleStates);
    }
    public void OnlySharedIdleStates()
    {
        _idleStates.Clear();
        _idleStates = new(_sharedIdleStates);
    }
    public void ClearIdleStates()
    {
        _idleStates.Clear();
    }
}
