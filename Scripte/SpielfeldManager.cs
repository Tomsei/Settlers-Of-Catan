using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpielfeldManager : MonoBehaviour
{
    public GameObject feldPrefab;               //prefab für das erstellen von feldern
    public GameObject[] inhaltePrefab;          //prefab für das erstellen fon inhalten
    public Feld[,] spielfeld;                   //refferenz auf alle spielferlder

    public float breitenMultiplier;
    public float höhenMultiplier;
    public float höhenoffset;

    public Texture2D spielfeldDarstellung;      //hält alle informationen über das spielfeld in farbe codiert
    private int höhe { get => spielfeldDarstellung.height; }
    private int breite { get => spielfeldDarstellung.width; }

    private void Start()
    {
        sortInhaltePrefabs();
        initSpielfeld();
    }

    /// <summary>
    /// initiallisiert das spielfeld ais der spielfeld darstellungs textur
    /// </summary>
    private void initSpielfeld()
    {
        spielfeld = new Feld[höhe,breite];
        for(int x = 0; x < höhe; x++)
        {
            for(int y = 0; y < breite; y++)
            {
                Color temp = spielfeldDarstellung.GetPixel(x, y);
                int id = getInhalteId(temp);
                if(id != -1)
                {
                    spielfeld[x, y] = Instantiate<GameObject>(feldPrefab).GetComponent<Feld>();
                    spielfeld[x, y].transform.parent = transform;
                    spielfeld[x, y].transform.localPosition = calculatePos(x, y);
                    spielfeld[x, y].initInhalt(inhaltePrefab[id]);
                    spielfeld[x, y].GetComponentInChildren<Renderer>().material.color = spielfeld[x, y].inhalt.item.Farbe;
                }
            }
        }
    }

    /// <summary>
    /// berechnet die position eines feldes aus den indices herraus
    /// </summary>
    /// <param name="x">
    /// der x indice
    /// </param>
    /// <param name="y">
    /// der y indice
    /// </param>
    /// <returns>
    /// die coordinaten das Feldes
    /// </returns>
    private Vector3 calculatePos(int _x, int _y)
    {
        float x = 0;
        float y = _x % 2 == 0 ? 0 : höhenoffset;

        x += _x * breitenMultiplier;
        y += _y * höhenMultiplier;

        return new Vector3(x, 0, y);
    }

    /// <summary>
    /// gibt die id des inhaltes zurück der diese farbe besitzt
    /// </summary>
    /// <param name="col">
    /// die zu suchende fabe
    /// </param>
    /// <returns>
    /// eine gültige id oder -1 wenn nicht gefunden
    /// </returns>
    private int getInhalteId(Color col)
    {
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
    /// -die item.id bestimmt den index
    /// </summary>
    private void sortInhaltePrefabs()
    {
        GameObject[] inhalteTemp = new GameObject[inhaltePrefab.Length];
        for (int i = 0; i < inhaltePrefab.Length; i++)
        {
            if (inhalteTemp[inhaltePrefab[i].GetComponent<FeldInhalt>().item.id] == null)
                inhalteTemp[inhaltePrefab[i].GetComponent<FeldInhalt>().item.id] = inhaltePrefab[i];
            else
                throw new System.Exception("Item Id's Übberlappen");
        }
        inhaltePrefab = inhalteTemp;
    }
}