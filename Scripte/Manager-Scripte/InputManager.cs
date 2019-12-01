using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * callback liste
 * 0 = Camera Zoom
 * 1 = Camera Translation
 * 2 = Camera Rotation
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
    public KeyCode CameraZoomInKey;
    public KeyCode CameraZoomOutKey;
    public bool CameraZoomMouseInvert;

    public KeyCode CamereMoveLKey;
    public KeyCode CamereMoveRKey;
    public KeyCode CamereMoveFKey;
    public KeyCode CamereMoveBKey;
    public int CameraMoveMouseButton;

    public KeyCode CamereRotateLKey;
    public KeyCode CamereRotateRKey;
    public KeyCode CamereRotateUKey;
    public KeyCode CamereRotateDKey;
    public int CameraRotateMouseButton;

    //Verwaltung der Inputs | Datenstruktur zum Speichern der Objekte die Inputs erhalten
    public Dictionary<int, IInputListener> callback;

    private Vector2 mousePositionDelta = new Vector2(-1,-1);
    private Vector2 mousePositionOld = new Vector2(-1,-1);

    /// <summary>
    /// Es werden die Eventabfragen aufgerufen
    /// </summary>
    void Update()
    {
        calculateMousePositionDelta();

        event0();
        event1();
        event2();
    }

    private void calculateMousePositionDelta()
    {
        if(mousePositionOld.x != -1)
        {
            mousePositionDelta.x = Input.mousePosition.x - mousePositionOld.x;
            mousePositionDelta.y = Input.mousePosition.y - mousePositionOld.y;
        }
        mousePositionOld.x = Input.mousePosition.x;
        mousePositionOld.y = Input.mousePosition.y;
    }

    /// <summary>
    /// Methode zum prüfen der Inputs für das Kamear Bewegen
    /// </summary>
    private void event2()
    {
        //Tastatur Eingabe
        if (Input.GetKey(CamereRotateLKey))
        {
            IInputListener obj = null;
            float[] param = { -1.0f, 0.0f };
            if (callback.TryGetValue(2, out obj))
                obj.callBack(2, param);
        }
        if (Input.GetKey(CamereRotateRKey))
        {
            IInputListener obj = null;
            float[] param = { 1.0f, 0.0f };
            if (callback.TryGetValue(2, out obj))
                obj.callBack(2, param);
        }
        if (Input.GetKey(CamereRotateUKey))
        {
            IInputListener obj = null;
            float[] param = { 0.0f, 1.0f };
            if (callback.TryGetValue(2, out obj))
                obj.callBack(2, param);
        }
        if (Input.GetKey(CamereRotateDKey))
        {
            IInputListener obj = null;
            float[] param = { 0.0f, -1.0f };
            if (callback.TryGetValue(2, out obj))
                obj.callBack(2, param);
        }

        if (Input.GetMouseButton(CameraRotateMouseButton))
        {
            IInputListener obj = null;
            float[] param = { mousePositionDelta.x, mousePositionDelta.y};
            if (callback.TryGetValue(2, out obj))
                obj.callBack(2, param);
        }
    }

    /// <summary>
    /// Methode zum prüfen der Inputs für das Kamear Bewegen
    /// </summary>
    private void event1()
    {
        //Tastatur Eingabe
        if (Input.GetKey(CamereMoveLKey))
        {
            IInputListener obj = null;
            float[] param = { -1.0f, 0.0f };
            if (callback.TryGetValue(1, out obj))
                obj.callBack(1, param);
        }
        if (Input.GetKey(CamereMoveRKey))
        {
            IInputListener obj = null;
            float[] param = { 1.0f, 0.0f };
            if (callback.TryGetValue(1, out obj))
                obj.callBack(1, param);
        }
        if (Input.GetKey(CamereMoveFKey))
        {
            IInputListener obj = null;
            float[] param = { 0.0f, 1.0f };
            if (callback.TryGetValue(1, out obj))
                obj.callBack(1, param);
        }
        if (Input.GetKey(CamereMoveBKey))
        {
            IInputListener obj = null;
            float[] param = { 0.0f, -1.0f };
            if (callback.TryGetValue(1, out obj))
                obj.callBack(1, param);
        }

        if (Input.GetMouseButton(CameraMoveMouseButton))
        {
            IInputListener obj = null;
            float[] param = { mousePositionDelta.x, -mousePositionDelta.y };
            if (callback.TryGetValue(1, out obj))
                obj.callBack(1, param);
        }
    }

    /// <summary>
    /// Methode zum prüfen der Inputs für das Kamear zoomen
    /// </summary>
    private void event0()
    {
        //Tastatur Eingabe
        if (Input.GetKey(CameraZoomInKey))
        {
            IInputListener obj = null;
            if (callback.TryGetValue(0, out obj))
                obj.callBack(0, -1);
        }
        if (Input.GetKey(CameraZoomOutKey))
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
                obj.callBack(0, (int)Input.mouseScrollDelta.y * (CameraZoomMouseInvert ? 1 : -1));
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

        /// <summary>
        /// Callback-Methode um ein einzelenen float[] abhängig vom Input zu übergeben
        /// </summary>
        /// <param name="eventId">ID des Events (welche Aufgabe wird gemacht)</param>
        /// <param name="value">der zu übergebende Wert</param>
        void callBack(int eventId, float[] value);
    }
}