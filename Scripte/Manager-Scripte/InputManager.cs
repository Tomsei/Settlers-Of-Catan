using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * callback liste
 * 0 = Camera Zoom
 */

/// <summary>
/// InputManager nimmt alle inputs entgegen und gibt diese, 
/// eventbasiert an die zugehörigen Objekte weiter
/// </summary>
public class InputManager : MonoBehaviour
{


    /// <summary>
    /// Region um das Singelton Pattern für den Input Manager zu erstellen
    /// </summary>
    #region singelton + callback init + Awake --> Region hat keinen Einfluss auf Programmen (nur zum Klappen)

    //Die Eine Instanz die existieren darf (Controller) / Nur eine
    public static InputManager controller;

    /// <summary>
    /// Awake ist quasi ein Konstruktor
    /// Setzt durch, dass nur eine Instanz des Input-Managers existieren kann
    /// </summary>
    public void Awake()
    {
        //prüfen obe es vorher eine gab
        if (controller != null)
        {
            Debug.LogError("Singelton schon vorhanden");
            throw new System.Exception("Singelton schon vorhanden");
        }

        controller = this;

        callback = new Dictionary<int, IInputListener>();   //Callback initialisieren 
    }

    #endregion




    //Inputs zum Zoomen
    public KeyCode zoomInKey;
    public KeyCode zoomOutKey;
    public bool zoomMouseInvert;


    //Verwaltung der Inputs | Datenstruktur zum Speichern der Objekte die Inputs erhalten
    public Dictionary<int, IInputListener> callback;


    /// <summary>
    /// Es werden die Eventabfragen aufgerufen
    /// </summary>
    void Update()
    {
        event0();
    }

    /// <summary>
    /// Methode zum prüfen der Inputs für das Kamear zoomen
    /// </summary>
    private void event0()
    {
        //Tastatur Eingabe
        if (Input.GetKey(zoomInKey))
        {
            IInputListener obj = null;
            if (callback.TryGetValue(0, out obj))
                obj.callBack(0, -1);
        }
        if (Input.GetKey(zoomOutKey))
        {
            IInputListener obj = null;
            if (callback.TryGetValue(0, out obj))
                obj.callBack(0, 1);
        }

        //Mausrad prüfung
        if (Input.mouseScrollDelta.y != 0)
        {
            //Es wird versucht ein Listener für das Kamera Zoomen zu erhalten
            IInputListener obj = null;
            if (callback.TryGetValue(0, out obj))
            {
                //Wenn es eine Kamera gibt die sich angemeldet hatt soll für diese Scrollen ausgefüht werden
                obj.callBack(0, (int)Input.mouseScrollDelta.y * (zoomMouseInvert ? 1 : -1));
            }
        }
    }

    /// <summary>
    /// Interface für alle Objekte die ein Input wollen
    /// behinhaltet die Methode callBack
    /// </summary>
    public interface IInputListener
    {
     
        /// <summary>
        /// Callback-Methode um ein einzelenen Integer abhängig vom Input zu übergeben
        /// </summary>
        /// <param name="eventId">ID des Events (welche Aufgabe wird gemacht)</param>
        /// <param name="value">der zu übergebende Wert</param>
        void callBack(int eventId, int value);
    }
}