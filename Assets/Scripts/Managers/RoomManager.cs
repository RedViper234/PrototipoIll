using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NavMeshPlus.Components;

public class RoomManager : Manager
{
    [Header("FIRST ROOM")]
    public AreaSO firstArea;
    [Header("CURRENT SETTINGS")]
    [MyReadOnly] public PuntoDiInteresse currentPoint;
    [MyReadOnly] public RoomData currentRoom;
    [HideInInspector] public AreaSO currentArea;
    public List<AreaSO> areeDaEsplorare;

    public UnityEvent OnAreaChanged;



    public void Start()
    {
        Initialize();
        StartCoroutine(SpawnRoom());
    }

    private void Initialize()
    {
        SetCurrentArea(firstArea);
        SetPuntiDiInteresse();
        SetFirstRoom();
    }

    public void SetCurrentArea(AreaSO newArea)
    {
        try
        {
            if (currentArea != newArea)
            {
                currentArea = newArea;
                OnAreaChanged.Invoke();
                Initialize();
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }
    public void SetPuntiDiInteresse()
    {
        for (int i = 0; i < currentArea.puntiDiInteresse.Count; i++)
        {
            if (currentArea.puntiDiInteresse[i] == null) { continue; }
            Debug.Log(currentArea.puntiDiInteresse[i]);
            for (int y = 0; y < currentArea.puntiDiInteresse[i].points.Count; y++)
            {
                currentPoint = currentArea.puntiDiInteresse[i];
            }
        }


    }
    private void SetFirstRoom()
    {
        // CONTROLLARE SE UNA STANZA HA IL FIRST ROOM SU TRUE
        if (currentPoint.roomPossibiliDaSpawnare.FindAll(x => x.isFirstRoom == true).Count > 0)
        {
            List<RoomData> rooms = currentPoint.roomPossibiliDaSpawnare.FindAll(x => x.isFirstRoom == true);
            if (rooms.Count > 1)
            {
                int roomRandomRange = UnityEngine.Random.Range(0, rooms.Count + 1);
                currentRoom = rooms[roomRandomRange];
            }
            else if (rooms.Count == 1)
            {
                currentRoom = rooms[0];
            }
        }
        else if (currentPoint.roomPossibiliDaSpawnare.FindAll(x => x.isFirstRoom == true).Count == 0)
        {
            List<RoomData> rooms = currentPoint.roomPossibiliDaSpawnare;
            int rangeForChoosingRoom = UnityEngine.Random.Range(0, rooms.Count + 1);
            currentRoom = rooms[rangeForChoosingRoom];
        }
    }

    public IEnumerator SpawnRoom()
    {
        yield return new WaitForFixedUpdate();
        try
        {
            GameObject roomSpawned = Instantiate(currentRoom.prefabStanza);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }

    public void VaiAlPuntoPrecedente(RoomData room)
    {

    }
    public void VaiAlPuntoSuccessivo(RoomData data)
    {

    }

}
