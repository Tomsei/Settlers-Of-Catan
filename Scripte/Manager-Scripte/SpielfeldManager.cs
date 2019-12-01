using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Kann Spielfeld erstellen
/// </summary>

public class SpielfeldManager : MonoBehaviour
{
    public GameObject feldPrefab;               //prefab für das erstellen von feldern
    public GameObject[] inhaltePrefab;          //prefab für das erstellen fon inhalten
    public Feld[,] spielfeld;                   //refferenz auf alle spielferlder


    public Texture2D spielfeldDarstellung;      //hält alle informationen über das spielfeld in farbe codiert
    private int höhe { get => spielfeldDarstellung.height; }    //Spielfeld Höhe + Breite, abhängig von Textur
    private int breite { get => spielfeldDarstellung.width; }   

    /// <summary>
    /// Wird beim Start der Szene aufgerufen
    ///
    /// </summary>
    private void Start()
    {
        sortInhaltePrefabs();
        initSpielfeld();
    }

    /// <summary>
    /// initiallisiert das spielfeld aus der spielfeld darstellungs textur
    /// </summary>
    private void initSpielfeld()
    {
        spielfeld = new Feld[höhe,breite];

        //geht über alle möglichen Felder
        for(int x = 0; x < höhe; x++)
        {
            for(int y = 0; y < breite; y++)
            {
                Color temp = spielfeldDarstellung.GetPixel(x, y);       //Texture Farbe ermitteln
                int id = getInhalteIdByColor(temp);                     //Id zur Farbe
                if(id != -1)
                {
                    spielfeld[x, y] = Instantiate<GameObject>(feldPrefab).GetComponent<Feld>();     //erstellt das Objekt Feld 
                    spielfeld[x, y].transform.parent = transform;                                   
                    spielfeld[x, y].transform.localPosition = calculatePos(x, y);                   //Position berechnen
                    spielfeld[x, y].initInhalt(inhaltePrefab[id]);                                  //Inhalt des Feldes initialisieren mit Item
                    spielfeld[x, y].GetComponentInChildren<Renderer>().material.color = spielfeld[x, y].inhalt.item.Farbe; //dem Feld eine Fabe zuweisen
                }
            }
        }
    }

    /// <summary>
    /// berechnet die position eines feldes aus den indizes herraus
    /// mit der benötigten "Verschiebung" um Felder passend anzuordnen
    /// </summary>
    /// <param name="_x">
    /// der x indize (Position)
    /// </param>
    /// <param name="_y">
    /// der y indize (Position)
    /// </param>
    /// <returns>
    /// die Koordinaten das Feldes
    /// </returns>
    private Vector3 calculatePos(int _x, int _y)
    {
        float x = 0;
        float y = (_x % 2 == 0) ? (0) : (0.86f);  //Verknüpft mit Abfrage | In Gerader Zeile -> 0 | nicht Gerade leicht versetzt

        x += _x * 1.5f;             //Verschiebung der Felder in die Höhe 
        y += _y * 1.73f;            //Verschiebung der Felder in die Breite

        return new Vector3(x, 0, y);
    }

    /// <summary>
    /// gibt die id des ersten inhaltes zurück der diese farbe besitzt
    /// </summary>
    /// <param name="col">
    /// die zu suchende fabe
    /// </param>
    /// <returns>
    /// eine gültige id oder -1 wenn nicht gefunden
    /// </returns>
    private int getInhalteIdByColor(Color col)
    {
        //geht über gesamtes Prefab Array und prüft und prüft nach Farbe
        for(int i = 0; i < inhaltePrefab.Length; i++)
        {
            if (inhaltePrefab[i].GetComponent<FeldInhalt>().item.Farbe.Equals(col))
                return i;
        }
        return -1;
    }

    /// <summary>
    /// sortiert das inhaltePrefab array nach der item id
    /// -wirft eine exception falls id's doppelt sind
    /// -die item.id bestimmt den index des PrefabsArray
    /// </summary>
    private void sortInhaltePrefabs()
    {

        GameObject[] inhalteTemp = new GameObject[inhaltePrefab.Length];
        
        //Schleife über alle Prefabs die geladen sind
        for (int i = 0; i < inhaltePrefab.Length; i++)
        {
            //Sofern Arrayplatz der Id noch frei ist, wird prefab an A.Platz geschrieben
            if (inhalteTemp[inhaltePrefab[i].GetComponent<FeldInhalt>().item.id] == null)
                inhalteTemp[inhaltePrefab[i].GetComponent<FeldInhalt>().item.id] = inhaltePrefab[i];
            else
                throw new System.Exception("Item Id's Übberlappen");
        }
        inhaltePrefab = inhalteTemp;            //Array sortiert setzen
    }
}