using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NavMeshPlus.Components;
using UnityEditor;
using UnityEngine.Tilemaps;

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
    [Header("READONLY VARIABLE")]
    [SerializeField, MyReadOnly] public int m_stanzeAttraversate = 0;


    private PlayerController m_playerControllerInstance;
    private FlagManager m_flagManager;
    public void Start()
    {
        m_playerControllerInstance = AppManager.Instance.playePrefabReference.GetComponent<PlayerController>();
        m_flagManager = AppManager.Instance?.flagManager;
        Initialize();
        SpawnCurrentRoom();
    }


    /// <summary>
    /// Inizializza il room manager impostando l'Area iniziale, e i punti di interesse e subito dopo
    /// una stanza per volta
    /// </summary>
    private void Initialize()
    {
        SetCurrentArea(firstArea);
        SetPuntiDiInteresse();
        SetRoom(true);

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

    /// <summary>
    /// La funzione serve a settare la stanza per questo punto, se checkFirstRoom è selezionato allora verrà
    /// fatto in modo di pescare, se possibile, una stanza che ha il booleano attivo first room
    /// </summary>
    /// <param name="checkFirstRoom"></param>
    private void SetRoom(bool checkFirstRoom)
    {
        m_stanzeAttraversate = m_stanzeAttraversate + 1;
        List<RoomData> stanzeCachateDaSO = currentPoint.roomPossibiliDaSpawnare;
        stanzeCachateDaSO = stanzeCachateDaSO.OrderByDescending(x => x.prioritaStanza).ToList();
        // CONTROLLARE SE UNA STANZA HA IL FIRST ROOM SU TRUE
        List<RoomData> roomFinaliCheRispettanoLeRegole = new ();
        foreach (var room in stanzeCachateDaSO)
        {
            if(CheckRequisiti(room))
            {
                roomFinaliCheRispettanoLeRegole.Add(room);
            }
        }
        roomFinaliCheRispettanoLeRegole.Sort((a, b) => b.prioritaStanza.CompareTo(a.prioritaStanza));
        int massimaPriorita = roomFinaliCheRispettanoLeRegole[0].prioritaStanza;
        roomFinaliCheRispettanoLeRegole = roomFinaliCheRispettanoLeRegole.FindAll(x => x.prioritaStanza == massimaPriorita);


        if (roomFinaliCheRispettanoLeRegole.Count > 1)
        {
            int temp = UnityEngine.Random.Range(0, roomFinaliCheRispettanoLeRegole.Count);
            currentRoom = roomFinaliCheRispettanoLeRegole[temp];
        }
        else if(roomFinaliCheRispettanoLeRegole.Count == 1)
        {
            currentRoom = roomFinaliCheRispettanoLeRegole[0];
        }
        Debug.Log($"<color=#00FF00>STANZA SELEZIONATA</color>");
        print(currentRoom.name);

        foreach (var item in currentRoom.flagsOnEnter.requiredFlagList)
        {
            bool hasFlag = m_flagManager.CheckFlag(item.flagList);
            if (hasFlag)
                m_flagManager.SetFlag(currentRoom.flagsOnEnter.flagToSet);
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
                    roomCanSpawn = ControlloNumeroStanzeAttraversate(requisiti);
                    break;
                case TipoRequisito.DistanzaPercorsa:
                    roomCanSpawn = true;
                    break;
                case TipoRequisito.ValoreDiStatistica:
                    roomCanSpawn = ControlloStatistiche(requisiti);
                    break;
                case TipoRequisito.QuantitaVitaMassima:
                    roomCanSpawn = ControlloQuantitaVitaMassima(requisiti);
                    break;
                case TipoRequisito.PercentualeVitaRimasta:
                    roomCanSpawn = ControlloPercentualeVitaPlayer(requisiti);
                    break;
                case TipoRequisito.Ustioni:
                    roomCanSpawn = ControlloQuantitaUstioni(requisiti);
                    break;
                case TipoRequisito.Flag:
                    roomCanSpawn = true;
                    break;
                default:
                    break;
            }
        }

        return roomCanSpawn;
    }    
    private bool CheckRequisiti(EnemySet enemySet) 
    {
        bool roomCanSpawn = false;
            switch (enemySet.tipoRequisito)
            {
                case TipoRequisito.PercentualeCorruzione:
                    roomCanSpawn = ControllaPercentualeCorruzione(enemySet);
                    break;
                case TipoRequisito.NumeroPoteriOttenuti:
                    roomCanSpawn = ControllaSeNumeroPoteriCorrisponde(enemySet);
                    break;
                case TipoRequisito.PoterSpecifico:
                    roomCanSpawn = true;
                    break;
                case TipoRequisito.NumeroStanzeAttraversate:
                    roomCanSpawn = ControlloNumeroStanzeAttraversate(enemySet);
                    break;
                case TipoRequisito.DistanzaPercorsa:
                    roomCanSpawn = true;
                    break;
                case TipoRequisito.ValoreDiStatistica:
                    roomCanSpawn = ControlloStatistiche(enemySet);
                    break;
                case TipoRequisito.QuantitaVitaMassima:
                    roomCanSpawn = ControlloQuantitaVitaMassima(enemySet);
                    break;
                case TipoRequisito.PercentualeVitaRimasta:
                    roomCanSpawn = ControlloPercentualeVitaPlayer(enemySet);
                    break;
                case TipoRequisito.Ustioni:
                    roomCanSpawn = ControlloQuantitaUstioni(enemySet);
                    break;
                case TipoRequisito.Flag:
                    roomCanSpawn = true;
                    break;
                default:
                    break;
            }

        return roomCanSpawn;
    }

    private bool ControlloNumeroStanzeAttraversate(RequisitiStanza requisiti)
    {
        bool roomCanSpawn = false;
        switch (requisiti.valoriPerEntrataStanze.operatori)
        {
            case OperatoriDiComparamento.Maggiore:
                roomCanSpawn = m_stanzeAttraversate > requisiti.valoriPerEntrataStanze.valore;
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = m_stanzeAttraversate >= requisiti.valoriPerEntrataStanze.valore;
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = m_stanzeAttraversate == requisiti.valoriPerEntrataStanze.valore;
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = m_stanzeAttraversate < requisiti.valoriPerEntrataStanze.valore;
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = m_stanzeAttraversate <= requisiti.valoriPerEntrataStanze.valore;
                break;
        }
        return roomCanSpawn;
    }

    private bool ControlloNumeroStanzeAttraversate(EnemySet set)
    {
        bool roomCanSpawn = false;
        switch (set.valoriPerEntrataStanze.operatori)
        {
            case OperatoriDiComparamento.Maggiore:
                roomCanSpawn = m_stanzeAttraversate > set.valoriPerEntrataStanze.valore;
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = m_stanzeAttraversate >= set.valoriPerEntrataStanze.valore;
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = m_stanzeAttraversate == set.valoriPerEntrataStanze.valore;
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = m_stanzeAttraversate < set.valoriPerEntrataStanze.valore;
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = m_stanzeAttraversate <= set.valoriPerEntrataStanze.valore;
                break;
        }
        return roomCanSpawn;
    }




    private bool ControlloQuantitaUstioni(RequisitiStanza requisiti)
    {
        bool roomCanSpawn = false;
        switch (requisiti.valoreQuantitaUstioni.operatori)
        {
            case OperatoriDiComparamento.Maggiore:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate > requisiti.valoreQuantitaUstioni.quantitaUstioni);
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate >= requisiti.valoreQuantitaUstioni.quantitaUstioni);
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate == requisiti.valoreQuantitaUstioni.quantitaUstioni);
                Debug.Log(roomCanSpawn);
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate < requisiti.valoreQuantitaUstioni.quantitaUstioni);
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate <= requisiti.valoreQuantitaUstioni.quantitaUstioni);
                break;
        }
        return roomCanSpawn;
    }


    private bool ControlloQuantitaUstioni(EnemySet set)
    {
        bool roomCanSpawn = false;
        switch (set.valoreQuantitaUstioni.operatori)
        {
            case OperatoriDiComparamento.Maggiore:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate > set.valoreQuantitaUstioni.quantitaUstioni);
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate >= set.valoreQuantitaUstioni.quantitaUstioni);
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate == set.valoreQuantitaUstioni.quantitaUstioni);
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate < set.valoreQuantitaUstioni.quantitaUstioni);
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate <= set.valoreQuantitaUstioni.quantitaUstioni);
                break;
        }
        return roomCanSpawn;
    }



    private bool ControlloQuantitaVitaMassima(RequisitiStanza requisiti)
    {
        bool roomCanSpawn = false;
        switch (requisiti.valoreQuantitaVitaMassima.operatori)
        {
            case OperatoriDiComparamento.Maggiore:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().maxHealth > requisiti.valoreQuantitaVitaMassima.quantita);
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().maxHealth >= requisiti.valoreQuantitaVitaMassima.quantita);
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().maxHealth == requisiti.valoreQuantitaVitaMassima.quantita);
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().maxHealth < requisiti.valoreQuantitaVitaMassima.quantita);
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().maxHealth <= requisiti.valoreQuantitaVitaMassima.quantita);
                break;
        }
        return roomCanSpawn;
    }

    private bool ControlloQuantitaVitaMassima(EnemySet set)
    {
        bool roomCanSpawn = false;
        switch (set.valoreQuantitaMassima.operatori)
        {
            case OperatoriDiComparamento.Maggiore:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().maxHealth > set.valoreQuantitaMassima.quantita);
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().maxHealth >= set.valoreQuantitaMassima.quantita);
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().maxHealth == set.valoreQuantitaMassima.quantita);
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().maxHealth < set.valoreQuantitaMassima.quantita);
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().maxHealth <= set.valoreQuantitaMassima.quantita);
                break;
        }
        return roomCanSpawn;
    }



    private bool ControlloPercentualeVitaPlayer(RequisitiStanza requisiti)
    {
        bool roomCanSpawn = false;
        switch(requisiti.valoreVitaRimasta.operatori)
        {
            case OperatoriDiComparamento.Maggiore:
                roomCanSpawn = (((m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth) * 100) > requisiti.valoreVitaRimasta.percentuale);
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = (((m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth) * 100) >= requisiti.valoreVitaRimasta.percentuale);
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = (((m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth) * 100) == requisiti.valoreVitaRimasta.percentuale);
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = (((m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth) * 100) < requisiti.valoreVitaRimasta.percentuale);
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = (((m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth) * 100) <= requisiti.valoreVitaRimasta.percentuale);
                break;
        }
        return roomCanSpawn;
    }
    private bool ControlloPercentualeVitaPlayer(EnemySet set)
    {
        bool roomCanSpawn = false;
        switch (set.valoreVitaRimasta.operatori)
        {
            case OperatoriDiComparamento.Maggiore:
                roomCanSpawn = (((m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth) * 100) > set.valoreVitaRimasta.percentuale);
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = (((m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth) * 100) >= set.valoreVitaRimasta.percentuale);
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = (((m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth) * 100) == set.valoreVitaRimasta.percentuale);
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = (((m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth) * 100) < set.valoreVitaRimasta.percentuale);
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = (((m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth) * 100) <= set.valoreVitaRimasta.percentuale);
                break;
        }
        return roomCanSpawn;
    }



    private bool ControlloStatistiche(RequisitiStanza requisiti)
    {
        bool roomCanSpawn = false;
        Dictionary<TipoStatistica, stat> statisticheDict = new Dictionary<TipoStatistica, stat>
        {
            { TipoStatistica.Strenght, m_playerControllerInstance.Strenght },
            { TipoStatistica.Speed, m_playerControllerInstance.Speed },
            { TipoStatistica.Aim, m_playerControllerInstance.Aim },
            { TipoStatistica.Constitution, m_playerControllerInstance.Constitution },
            { TipoStatistica.Luck, m_playerControllerInstance.Luck }
        };

        // Verifica se il tipo di statistica esiste nel dizionario
        if (statisticheDict.TryGetValue(requisiti.valoriStatistichePersonaggio.tipoStatistica, out stat statistica))
        {
            switch (requisiti.valoriStatistichePersonaggio.operatori)
            {
                case OperatoriDiComparamento.Maggiore:
                    roomCanSpawn = statistica.livello > requisiti.valoriStatistichePersonaggio.valoreStatistica;
                    break;
                case OperatoriDiComparamento.MaggioreOUguale:
                    roomCanSpawn = statistica.livello >= requisiti.valoriStatistichePersonaggio.valoreStatistica;
                    break;
                case OperatoriDiComparamento.Uguale:
                    roomCanSpawn = statistica.livello == requisiti.valoriStatistichePersonaggio.valoreStatistica;
                    break;
                case OperatoriDiComparamento.Minore:
                    roomCanSpawn = statistica.livello < requisiti.valoriStatistichePersonaggio.valoreStatistica;
                    break;
                case OperatoriDiComparamento.MinoreOUguale:
                    roomCanSpawn = statistica.livello <= requisiti.valoriStatistichePersonaggio.valoreStatistica;
                    break;
            }
        }
        return roomCanSpawn;
         // Ritorna false se il tipo di statistica non è valido
    }
    private bool ControlloStatistiche(EnemySet set)
    {
        bool roomCanSpawn = false;
        Dictionary<TipoStatistica, stat> statisticheDict = new Dictionary<TipoStatistica, stat>
        {
            { TipoStatistica.Strenght, m_playerControllerInstance.Strenght },
            { TipoStatistica.Speed, m_playerControllerInstance.Speed },
            { TipoStatistica.Aim, m_playerControllerInstance.Aim },
            { TipoStatistica.Constitution, m_playerControllerInstance.Constitution },
            { TipoStatistica.Luck, m_playerControllerInstance.Luck }
        };

        // Verifica se il tipo di statistica esiste nel dizionario
        if (statisticheDict.TryGetValue(set.valoriStatistichePersonaggio.tipoStatistica, out stat statistica))
        {
            switch (set.valoriStatistichePersonaggio.operatori)
            {
                case OperatoriDiComparamento.Maggiore:
                    roomCanSpawn = statistica.livello > set.valoriStatistichePersonaggio.valoreStatistica;
                    break;
                case OperatoriDiComparamento.MaggioreOUguale:
                    roomCanSpawn = statistica.livello >= set.valoriStatistichePersonaggio.valoreStatistica;
                    break;
                case OperatoriDiComparamento.Uguale:
                    roomCanSpawn = statistica.livello == set.valoriStatistichePersonaggio.valoreStatistica;
                    break;
                case OperatoriDiComparamento.Minore:
                    roomCanSpawn = statistica.livello < set.valoriStatistichePersonaggio.valoreStatistica;
                    break;
                case OperatoriDiComparamento.MinoreOUguale:
                    roomCanSpawn = statistica.livello <= set.valoriStatistichePersonaggio.valoreStatistica;
                    break;
            }
        }
        return roomCanSpawn;
        // Ritorna false se il tipo di statistica non è valido
    }


    private bool ControllaSeNumeroPoteriCorrisponde(RequisitiStanza requisiti)
    {
        // DA ASPETTARE POTERI 
        return true;
    }
    private bool ControllaSeNumeroPoteriCorrisponde(EnemySet set)
    {
        // DA ASPETTARE POTERI 
        return true;
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
    private bool ControllaPercentualeCorruzione(EnemySet set)
    {
        bool temp = false;
        switch (set.valoreCorruzione.operatori)
        {
            case OperatoriDiComparamento.Maggiore:
                temp = (set.valoreCorruzione.valoreDaComparare > m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione);
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                temp = (set.valoreCorruzione.valoreDaComparare >= m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione);
                break;
            case OperatoriDiComparamento.Uguale:
                temp = (set.valoreCorruzione.valoreDaComparare == m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione);
                break;
            case OperatoriDiComparamento.Minore:
                temp = (set.valoreCorruzione.valoreDaComparare < m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione);
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                temp = (set.valoreCorruzione.valoreDaComparare <= m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione);
                break;
            default:
                break;
        }
        return temp;
    }













    // ROOMS
    public void SpawnCurrentRoom()
    {
        try
        {
            GameObject roomSpawned = Instantiate(currentRoom.prefabStanza);
            GameObject playerSpawnato = SetPlayerInRoom(roomSpawned);
            AppManager.Instance.SetPlayerObject(playerSpawnato);
            m_playerControllerInstance = playerSpawnato.GetComponent<PlayerController>();
            AppManager.Instance.SetCameraPlayer();
            if ((currentRoom.tipiDiStanza & TipiDiStanza.Combattimento) != 0)
            {
                List<EnemySet> validEnemyList = new List<EnemySet>();
                validEnemyList = GetValidEnemySet(currentRoom.setDiMostriDellaStanza);
                SetEnemySet(validEnemyList);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }



    private GameObject SetPlayerInRoom(GameObject roomSpawned)
    {
        Room roomScript = roomSpawned.GetComponent<Room>();
        GameObject playerSpawned = new GameObject();
        int range = 0;
        if (roomScript != null)
        {
            if(roomScript.puntiDiSpawnPossibili.Count > 1)
            {
                range = UnityEngine.Random.Range(0, roomScript.puntiDiSpawnPossibili.Count);
                playerSpawned = Instantiate(AppManager.Instance.playePrefabReference, roomScript.puntiDiSpawnPossibili[range].localPosition,Quaternion.identity);
                print("<color=#00000>PLAYER SPAWNATO</color>");
            }
            else if(roomScript.puntiDiSpawnPossibili.Count == 1)
            {
                playerSpawned = Instantiate(AppManager.Instance.playePrefabReference, roomScript.puntiDiSpawnPossibili[0].localPosition, Quaternion.identity);
            }
            else if(roomScript.puntiDiSpawnPossibili.Count == 0 || roomScript.puntiDiSpawnPossibili == null)
            {
                Grid gridComponent = roomSpawned.GetComponent<Grid>();
                Tilemap tilemap = gridComponent.GetComponentInChildren<Tilemap>();

                if (tilemap != null)
                {
                    BoundsInt bounds = tilemap.cellBounds;

                    Vector3 cellSize = tilemap.cellSize;
                    Vector3 gridMin = tilemap.GetCellCenterWorld(bounds.min);
                    Vector3 gridMax = tilemap.GetCellCenterWorld(bounds.max);

                    // Genera un punto casuale all'interno delle dimensioni della tilemap
                    float randomX = UnityEngine.Random.Range(gridMin.x, gridMax.x);
                    float randomY = UnityEngine.Random.Range(gridMin.y, gridMax.y);

                    // Crea il punto casuale
                    Vector3 randomPointInTilemap = new Vector3(randomX, randomY, 0f);
                }

            }
        }
        return playerSpawned;
    }

    public List<EnemySet> GetValidEnemySet(List<EnemySet> setDiMostriDellaStanza)
    {
        List<EnemySet> enemySetValidiAiRequisiti = new List<EnemySet>();
        foreach (var setDiMostri in setDiMostriDellaStanza)
        {
            if (CheckRequisiti(setDiMostri))
            {
                enemySetValidiAiRequisiti.Add(setDiMostri);
                Debug.Log($"<b><color=#FF00e1>MONSTER SET VALIDO: {setDiMostri}</color> <quad=2></b>");
               
            }
            else
            {
                Debug.Log($"<b><color=#FF0000>MONSTER SET NON VALIDO: {setDiMostri}</color> <quad=2></b>");
            }
        }
        if(setDiMostriDellaStanza.Count == 0)
        {
            foreach (var setMostriArea in currentArea.enemySet)
            {
                if (CheckRequisiti(setMostriArea))
                {
                    enemySetValidiAiRequisiti.Add(setMostriArea);
                    Debug.Log($"<b><color=#FF00e1>MONSTER SET VALIDO: {setMostriArea}</color> <quad=2></b>");
                }
            }
        }
        if(enemySetValidiAiRequisiti.Count == 0)
        {
            int rangeRandomPerSpwanForzatoSet = UnityEngine.Random.Range(0,currentArea.enemySet.Count);
            enemySetValidiAiRequisiti.Add(currentArea.enemySet[rangeRandomPerSpwanForzatoSet]);
            Debug.LogError("Zero enemy set validi, metti almeno un'enemy set valido.\n E stato scelto un set casuale dall'area in modo forzato, risolvere");
        }
        return setDiMostriDellaStanza;
    }
    public void SetEnemySet(List<EnemySet> enemySets)
    {
        Debug.Log(enemySets);
    }
}
