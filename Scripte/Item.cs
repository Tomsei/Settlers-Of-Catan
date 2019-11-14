using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sturktur zum abspeichern von Rohstoffen 
/// </summary>

[System.Serializable]
public struct Item
{
    public string name;
    public Color Farbe;         //zur spielfeld generierung (Pixelfarbe)
    public int id;
}