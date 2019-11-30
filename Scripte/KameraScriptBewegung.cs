using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraScriptBewegung : MonoBehaviour , InputManager.IListener
{

    public Transform pointer;

    public float zoomScaler;
    public float zoomSmothScaler;
    public float zoomMinDist;
    public float zoomMaxDist;

    private float pointerDistance = 50;
    
    public void Start()
    {
        InputManager.controller.callback.Add(0, this);
    }

    private void Update()
    {
        transform.LookAt(pointer);

        updateDistance();
    }

    private void updateDistance()
    {
        float dist = Vector3.Distance(pointer.position,transform.position);
        float differenz = dist - pointerDistance;

        transform.position += transform.forward * differenz * Time.deltaTime * zoomSmothScaler;
    }

    public void callBack(int eventId, int value)
    {
        switch (eventId)
        {
            case 0:
                zoom(value);
                break;

            default:
                Debug.Log("Nicht zugewiesenes Event " + eventId);
                break;
        }
    }

    /// <summary>
    /// zoom zum pointer
    /// </summary>
    /// <param name="ScrollWert">
    /// wenn positiv dann entfernen
    /// wenn negativ dann ranzoomen
    /// </param>
    public void zoom(int ScrollWert)
    {
        pointerDistance += ScrollWert * zoomScaler;
        if (pointerDistance < zoomMinDist)
            pointerDistance = zoomMinDist;
        if (pointerDistance > zoomMaxDist)
            pointerDistance = zoomMaxDist;
    }
}