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

    [Expandable]
    public AreaSO firstArea;
    [Header("CURRENT SETTINGS")]
    [MyReadOnly] public PuntoDiInteresse currentPoint;
    [MyReadOnly] public RoomData currentRoom;
    [HideInInspector] public AreaSO currentArea;
    [Expandable]
    public List<AreaSO> areeDaEsplorare;
    
    public UnityEvent OnAreaChanged;

    private PlayerController m_playerControllerInstance;

    public void Start()
    {
        m_playerControllerInstance = AppManager.Instance?.playerControllerInstance;
        Initialize();
        StartCoroutine(SpawnCurrentRoom());
    }


    /// <summary>
    /// Inizializza il room manager impostando l'Area iniziale, e i punti di interesse e subito dopo
    /// una stanza per volta
    /// </summary>
    private void Initialize()
    {
        SetCurrentArea(firstArea);
        SetPuntiDiInteresse();
        SetFirstRoom();
    }

    // FUNZIONI AREE
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

    //FUNZIONI PUNTI DI INTERESSE
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
    public void VaiAlPuntoPrecedente(RoomData room)
    {

    }
    public void VaiAlPuntoSuccessivo(RoomData data)
    {

    }

    // FUNZIONI STANZE
    private void SetFirstRoom()
    {
        // CONTROLLARE SE UNA STANZA HA IL FIRST ROOM SU TRUE
        if (currentPoint.roomPossibiliDaSpawnare.FindAll(x => x.isFirstRoom == true).Count > 0)
        {
            List<RoomData> rooms = currentPoint.roomPossibiliDaSpawnare.FindAll(x => x.isFirstRoom == true);
            foreach (var room in rooms)
            {
                if (!CheckRequisiti(room))
                {
                    rooms.Remove(room);
                }
                else
                {
                    continue;
                }

            }
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

    //CHECK REQUISITI
    private bool CheckRequisiti(RoomData room) 
    {
        bool roomCanSpawn = false;
        for (int i = 0; i < room.requisitiStanza.Count; i++)
        {
            var requisiti = room.requisitiStanza[i];
            switch (requisiti.tipoRequisito)
            {
                case TipoRequisito.PercentualeCorruzione:
                    roomCanSpawn = ControllaPercentualeCorruzione(requisiti);
                    break;
                case TipoRequisito.NumeroPoteriOttenuti:
                    roomCanSpawn = ControllaSeNumeroPoteriCorrisponde(requisiti);
                    break;
                case TipoRequisito.PoterSpecifico:
                    roomCanSpawn = true;
                    break;
                case TipoRequisito.NumeroStanzeAttraversate:
                    break;
                case TipoRequisito.DistanzaPercorsa:
                    break;
                case TipoRequisito.ValoreDiStatistica:
                    break;
                case TipoRequisito.Flag:
                    break;
                default:
                    break;
            }
        }

        return roomCanSpawn;
    }

    private bool ControllaSeNumeroPoteriCorrisponde(RequisitiStanza requisiti)
    {
        // DA ASPETTARE POTERI 
        return false;
    }

    private bool ControllaPercentualeCorruzione(RequisitiStanza requisiti)
    {
        bool temp = false;
        switch (requisiti.valoreCorruzione.operatori)
        {
            case OperatoriDiComparamento.Maggiore:
                temp = (requisiti.valoreCorruzione.valoreDaComparare > m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione);
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                temp = (requisiti.valoreCorruzione.valoreDaComparare >= m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione);
                break;
            case OperatoriDiComparamento.Uguale:
                temp = (requisiti.valoreCorruzione.valoreDaComparare == m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione);
                break;
            case OperatoriDiComparamento.Minore:
                temp = (requisiti.valoreCorruzione.valoreDaComparare < m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione);
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                temp = (requisiti.valoreCorruzione.valoreDaComparare <= m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione);
                break;
            default:
                break;
        }
        return temp;
    }












    // ROOMS
    public void SetCurrentRoom(RoomData newCurrentRoom)
    {

    }
    public IEnumerator SpawnCurrentRoom()
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



}
