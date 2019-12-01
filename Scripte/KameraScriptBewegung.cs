using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Klasse zum behandeln der gesamten Kamera Bewegung
/// Implementiert IInputListener um Input-Werte zu erhalten
/// </summary>
public class KameraScriptBewegung : MonoBehaviour , InputManager.IInputListener
{
    //Der Punkt auf den die Kamera schaut 
    public Transform pointer;

    //Einstellungen des Zoomens
    public float zoomScaler;
    public float zoomSmothScaler;               //Wie viele änderungen pro Frame am Wert
    public float zoomMinDist;
    public float zoomMaxDist;

    //Ziel Entfernung der Kamera
    private float pointerSollDistance = 50;
    

    /// <summary>
    /// Anmeldung für die Callbacks (Input...)
    /// </summary>
    public void Start()
    {
        InputManager.controller.callback.Add(0, this);          //Liste des InputManagers
    }

    /// <summary>
    /// Alle Frame-Abhängigen Kamera Bewegungen ausführen
    /// </summary>
    private void Update()
    {
        //Drehung zum Pointer
        transform.LookAt(pointer);

        updateDistance();
    }

    /// <summary>
    /// Methode ist zuständig für das zoomen
    /// Sie veränder die Distanz zum Pointer
    /// </summary>
    private void updateDistance()
    {
        //Momentane Distanz zwischen Pointer + Kamera (Achsenunabhängig)
        float dist = Vector3.Distance(pointer.position,transform.position);
        //Unterschied zwischen momentaner + soll Distanz
        float differenz = dist - pointerSollDistance;

        //transform.forward: Ein Schritt in eigener Rotation vorwärts
        //zoomSmothScaler: Multiplikator zur Steuerung
        transform.position += transform.forward * differenz * Time.deltaTime * zoomSmothScaler;
    }


    /// <summary>
    /// Interface Methode Callback
    /// Über Switch Case Auswhälen welches Event der Kamera ausgefürht werden soll
    /// </summary>
    /// <param name="eventId">ID des Events (welche Aufgabe wird gemacht)</param>
    /// <param name="value">der zu übergebende Wert</param>
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
    /// Aktualisierung des Sollwertes der Distanz beim Zoomen
    /// --> durch aktualisierten Wert hat updateDistance auswirkungen
    /// </summary>
    /// <param name="ScrollWert">
    /// wenn positiv dann Sollwert erhöhen --> später raussumen
    /// wenn negativ dann Sollwert verringern --> später reinsumen
    /// </param>
    public void zoom(int ScrollWert)
    {
        pointerSollDistance += ScrollWert * zoomScaler;
        if (pointerSollDistance < zoomMinDist)
            pointerSollDistance = zoomMinDist;
        if (pointerSollDistance > zoomMaxDist)
            pointerSollDistance = zoomMaxDist;
    }
}