using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Gibt die Form eines Feldes an
///  Enthällt einen Feldinhalt, der gesetzt werden kann
/// </summary>

public class Feld : MonoBehaviour
{
    public FeldInhalt inhalt;

    /// <summary>
    /// Methode zum Zuweisen eines Feldinhaltes
    /// Inhalt instanziieren und im Anschluss zuweisen
    /// </summary>
    /// <param name="perfab">das zu initialiesierende prefab</param>
    public void initInhalt(GameObject perfab)
    {
        GameObject temp = Instantiate<GameObject>(perfab);
        temp.transform.parent = this.transform;                 //Bewegungen synchronisieren
        temp.transform.localPosition = Vector3.zero;            //Position im Feld an richtige Stelle
        temp.transform.localRotation = Quaternion.identity;     //Rotierung ausrichten
        inhalt = temp.GetComponent<FeldInhalt>();
    }
}
