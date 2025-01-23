using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPosition
{
    private Vector3 position;

    private Vector3 overlap;
    private Vector3 overLappingPosition;
    public SpawnPosition(Vector3 _position, Vector3 _overlap)
    {
        position = _position;
        overlap = _overlap;
    }
    public SpawnPosition(Vector3 _position, Vector3 _overlap, Vector3 _overLappingPosition)
    {
        position = _position;
        overlap = _overlap; 
        overLappingPosition = _overLappingPosition;
    }
    public  Vector3 Position{
        get { return position; }
        set { position = value; }
    
    }


    public Vector3 Overlap
    {
        get { return overlap; }

        set { overlap = value; }

    }

    public Vector3 OverlappingPosition
    {
        get { return overLappingPosition; }

        set { overLappingPosition = value; }

    }





}
