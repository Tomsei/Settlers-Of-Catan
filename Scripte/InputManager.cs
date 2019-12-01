using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * callback liste
 * 0 = Camera Zoom
 */

/// <summary>
/// nimmt alle inputs entgegen und verteilt diese
/// </summary>
public class InputManager : MonoBehaviour
{

    #region singelton + callback init + Awake

    public static InputManager controller;

    public void Awake()
    {
        if (controller != null)
        {
            Debug.LogError("Singelton schon vorhanden");
            throw new System.Exception("Singelton schon vorhanden");
        }

        controller = this;

        callback = new Dictionary<int, IListener>();
    }

    #endregion

    public KeyCode zoomInKey;
    public KeyCode zoomOutKey;
    public bool zoomMouseInvert;

    public Dictionary<int, IListener> callback;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(zoomInKey))
        {
            IListener obj = null;
            callback.TryGetValue(0, out obj);
            obj.callBack(0, -1);
        }
        if (Input.GetKey(zoomOutKey))
        {
            IListener obj = null;
            callback.TryGetValue(0, out obj);
            obj.callBack(0, 1);
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            IListener obj = null;
            callback.TryGetValue(0, out obj);
            obj.callBack(0, (int)Input.mouseScrollDelta.y);
        }
    }

    public interface IListener
    {
        void callBack(int eventId, int value);
    }
}