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

    public float moveScaler;
    public float moveSmothScaler;
    public float moveXPosMax;
    public float moveXPosMin;
    public float moveZPosMax;
    public float moveZPosMin;

    public float rotateScaler;
    public float rotateSmothScaler;
    public float rotateVertikalMaxWinkel;
    public float rotateVertikalMinWinkel;

    //Ziel Entfernung der Kamera
    private float pointerSollDistance = 50;

    private float moveXSollPos = 0;
    private float moveZSollPos = 0;

    private float rotateHorrizontalSollWinkel = 0;
    public float rotateVertikaleSollWinkel = 45;


    /// <summary>
    /// Anmeldung für die Callbacks (Input...)
    /// </summary>
    public void Start()
    {
        InputManager.controller.callback.Add(0, this);          //Liste des InputManagers
        InputManager.controller.callback.Add(1, this);
        InputManager.controller.callback.Add(2, this);
    }

    /// <summary>
    /// Alle Frame-Abhängigen Kamera Bewegungen ausführen
    /// </summary>
    private void Update()
    {
        //Drehung zum Pointer
        transform.LookAt(pointer);

        updateRotation();
        updatePosition();
        updateDistance();
    }

    #region Zoomen

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

    #endregion

    #region move

    /// <summary>
    /// Aktualisierung des Sollwertes der Position beim Bewegen
    /// </summary>
    /// <param name="MoveWert">
    /// 0 = r/l achse positiv right
    /// 1 = f/b achse positiv forward
    /// </param>
    private void Move(float[] MoveWert)
    {
        Vector2 delta = new Vector2(MoveWert[1], MoveWert[0]);

        moveXSollPos += delta.x * moveScaler;
        if (moveXSollPos > moveXPosMax)
            moveXSollPos = moveXPosMax;
        if (moveXSollPos < moveXPosMin)
            moveXSollPos = moveXPosMin;

        moveZSollPos += delta.y * moveScaler;
        if (moveZSollPos > moveZPosMax)
            moveZSollPos = moveZPosMax;
        if (moveZSollPos < moveZPosMin)
            moveZSollPos = moveZPosMin;
    }

    private void updatePosition()
    {
        float differenzX = moveXSollPos - pointer.transform.position.x;
        float differenzZ = moveZSollPos - pointer.transform.position.z;

        transform.position += new Vector3(differenzX, 0, differenzZ) * Time.deltaTime * moveSmothScaler;
        pointer.transform.position += new Vector3(differenzX, 0, differenzZ) * Time.deltaTime * moveSmothScaler;
    }

    #endregion

    #region rotation

    private void updateRotation()
    {
        float dist = Vector3.Distance(transform.position, pointer.transform.position);

        float hoehenKomponenteVertikal = Mathf.Sin((rotateVertikaleSollWinkel / 360) * 2 * Mathf.PI);
        float laengenKomponenteVertikal = Mathf.Cos((rotateVertikaleSollWinkel / 360) * 2 * Mathf.PI);

        float breitenKomponenteHorrizontal = Mathf.Sin((rotateHorrizontalSollWinkel / 360) * 2 * Mathf.PI);
        float laengenKomponenteHorrizontal = Mathf.Cos((rotateHorrizontalSollWinkel / 360) * 2 * Mathf.PI);

        float xPos = breitenKomponenteHorrizontal * laengenKomponenteVertikal + pointer.transform.position.x;
        float yPos = hoehenKomponenteVertikal + pointer.transform.position.y;
        float zPos = laengenKomponenteHorrizontal * laengenKomponenteVertikal + pointer.transform.position.x;

        float xdiff = xPos * dist - transform.position.x;
        float ydiff = yPos * dist - transform.position.y;
        float zdiff = zPos * dist - transform.position.z;

        transform.position += new Vector3(xdiff * Time.deltaTime * rotateSmothScaler, ydiff * Time.deltaTime * rotateSmothScaler, zdiff * Time.deltaTime * rotateSmothScaler);
    }

    /// <summary>
    /// rotiert die kammera um den pointer
    /// </summary>
    /// <param name="MoveWert">
    /// 0 = horrizontal positiv rechts
    /// 1 = vertikal poitiv hoch
    /// </param>
    private void Rotate(float[] MoveWert)
    {
        rotateHorrizontalSollWinkel += MoveWert[0] * rotateScaler;
        rotateVertikaleSollWinkel += MoveWert[1] * rotateScaler;

        if (rotateVertikaleSollWinkel > rotateVertikalMaxWinkel)
            rotateVertikaleSollWinkel = rotateVertikalMaxWinkel;
        if (rotateVertikaleSollWinkel < rotateVertikalMinWinkel) 
            rotateVertikaleSollWinkel = rotateVertikalMinWinkel;
    }

    #endregion

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

    public void callBack(int eventId, float[] value)
    {
        switch (eventId)
        {
            case 1:
                Move(value);
                break;
            case 2:
                Rotate(value);
                break;

            default:
                Debug.Log("Nicht zugewiesenes Event " + eventId);
                break;
        }
    }

}