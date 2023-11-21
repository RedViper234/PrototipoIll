using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NavMeshPlus.Components;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor;
using AYellowpaper.SerializedCollections;



public class RoomManager : Manager
{
    [SerializedDictionary("PuntoDiInteresse", "RoomData")]

    [Header("FIRST ROOM")]

    [Expandable]
    public AreaSO firstArea;
    [Header("CURRENT SETTINGS")]
    [MyReadOnly] public PuntoDiInteresse currentPoint;
    [MyReadOnly] public RoomData currentRoom;
    [MyReadOnly] public AreaSO currentArea = null;
    [MyReadOnly] public EnemySet currentEnemySet;
    [MyReadOnly, SerializeField] private int m_enemyQuantity;
    [Expandable]
    public List<AreaSO> areeDaEsplorare;
    public SerializedDictionary<PuntoDiInteresse, RoomData> m_puntiDiInteresseSpawnatiNellArea;
    public UnityEvent OnAreaChanged;
    [Header("READONLY VARIABLE")]
    [SerializeField, MyReadOnly] public int m_stanzeAttraversate = 0;
    [SerializeField, MyReadOnly] private int m_indiceOndataCorrente = 0;
    [SerializeField, MyReadOnly] private int m_indicePuntoDiInteresseCorrente = 0;


    private FlagManager m_flagManager;
    private PlayerController m_playerControllerInstance;
    private NavMeshSurface m_surface;
    private Vector2 m_lastMonsterSpawnPosition;

    public void Update()
    {

    }
    public IEnumerator Start()
    {
        m_puntiDiInteresseSpawnatiNellArea = new SerializedDictionary<PuntoDiInteresse, RoomData>();
        m_indiceOndataCorrente = 0;
        m_playerControllerInstance = AppManager.Instance.playePrefabReference.GetComponent<PlayerController>();
        m_flagManager = AppManager.Instance?.flagManager;
        var coroutineDiInizializzazione = this.RunCoroutine(Initialize());
        SpawnCurrentRoom();
        yield return new WaitUntil(() => coroutineDiInizializzazione.IsDone);

    }


    /// <summary>
    /// Inizializza il room manager impostando l'Area iniziale, e i punti di interesse e subito dopo
    /// una stanza per volta
    /// </summary>
    private IEnumerator Initialize()
    {
        var coroutineArea = this.RunCoroutine(SetCurrentArea(firstArea,false));
        currentPoint = currentArea.puntiDiInteresse.FirstOrDefault();
        var coroutinePuntiDiInteresse = this.RunCoroutine(CostruisciTuttiIPuntiDiInteresseDellArea()); 
        var coroutineSpawnStanza = this.RunCoroutine(SetRoom(true));
        yield return new WaitUntil(() => coroutineArea.IsDone && coroutinePuntiDiInteresse.IsDone && coroutineSpawnStanza.IsDone);
    }



    // FUNZIONI AREE
    public IEnumerator SetCurrentArea(AreaSO newArea,bool firstTime = false)
    {
        if (currentArea != newArea)
        {
            Debug.Log($"<color=red>SetCurrentArea {newArea.name}</color>");
            CoroutineHandle coroutinePuntiDiInteresse = null;
            CoroutineHandle coroutineSpawnStanza = null;
            OnAreaChanged.Invoke();
            currentArea = newArea;
            currentPoint = currentArea.puntiDiInteresse.FirstOrDefault();
            if(currentPoint != null){
                /* DEVO DIFFERENZIARE TRA LA PRIMA VOLTA CHE SI ENTRA NELL'AREA E LE ALTRE VOLTE. PERCHE' LA PRIMA VOLTA SI DECIDE LA PRIMA STANZA E PUNTO CORRENTE
                MENTRE LA SECONDA VOLTA SI COSTRUISCONO LE SOTTOAREE E I RELATIVI PUNTI DI INTERESSE*/
                coroutinePuntiDiInteresse = this.RunCoroutine(CostruisciTuttiIPuntiDiInteresseDellArea()); 
                coroutineSpawnStanza = this.RunCoroutine(SetRoom(true));
                yield return new WaitUntil(() => coroutinePuntiDiInteresse.IsDone && coroutineSpawnStanza.IsDone);
                if(currentArea.sottoAree.Count > 0){
                    foreach (AreaSO sottoArea in currentArea.sottoAree){
                        CoroutineHandle coroutineSottoArea = this.RunCoroutine(SetCurrentArea(sottoArea));
                        yield return new WaitUntil(() => coroutineSottoArea.IsDone);
                    }
                }
            }
            else{
                // ALLORA FAI IN MODO CHE L'AREA SCELGA TUTTO QUANTO PER QUANTO RIGUARDA IL PUNTO DI INTERESSE
            }
        }
    }
    public IEnumerator SetCurrentArea(AreaSO newArea){
        if (currentArea != newArea)
        {
            currentArea = newArea;
            Debug.Log($"<color=red>SetCurrentArea {newArea.name}</color>");
            CoroutineHandle coroutinePuntiDiInteresse = null;
            OnAreaChanged.Invoke();
            if(currentPoint != null){
                /* DEVO DIFFERENZIARE TRA LA PRIMA VOLTA CHE SI ENTRA NELL'AREA E LE ALTRE VOLTE. PERCHE' LA PRIMA VOLTA SI DECIDE LA PRIMA STANZA E PUNTO CORRENTE
                MENTRE LA SECONDA VOLTA SI COSTRUISCONO LE SOTTOAREE E I RELATIVI PUNTI DI INTERESSE*/
                coroutinePuntiDiInteresse = this.RunCoroutine(CostruisciTuttiIPuntiDiInteresseDellArea(currentArea)); 
                yield return new WaitUntil(() => coroutinePuntiDiInteresse.IsDone);
                if(currentArea.sottoAree.Count > 0){
                    foreach (AreaSO sottoArea in currentArea.sottoAree){
                        CoroutineHandle coroutineSottoArea = this.RunCoroutine(SetCurrentArea(sottoArea));
                        yield return new WaitUntil(() => coroutineSottoArea.IsDone);
                    }
                }
            }
            else{
                // ALLORA FAI IN MODO CHE L'AREA SCELGA TUTTO QUANTO PER QUANTO RIGUARDA IL PUNTO DI INTERESSE
            }
        }
    }

    //FUNZIONI PUNTI DI INTERESSE
    public IEnumerator CostruisciTuttiIPuntiDiInteresseDellArea()
    {
        for (int i = 0; i < currentArea.puntiDiInteresse.Count; i++)
        {
            PuntoDiInteresse punto = currentArea.puntiDiInteresse[i];
            CostruisciPuntoDiInteresse(punto, 0);
            yield return null;
        }
    }
    public IEnumerator CostruisciTuttiIPuntiDiInteresseDellArea(AreaSO area)
    {
        for (int i = 0; i < area.puntiDiInteresse.Count; i++)
        {
            PuntoDiInteresse punto = area.puntiDiInteresse[i];
            CostruisciPuntoDiInteresse(punto, 0);
            yield return null;
        }
    }



    private void CostruisciPuntoDiInteresse(PuntoDiInteresse punto, int level)
    {
        var room = SetRoom(punto, true);
        if (m_puntiDiInteresseSpawnatiNellArea.ContainsKey(punto)) { return; }
        m_puntiDiInteresseSpawnatiNellArea.Add(punto, room);
        Publisher.Publish(new CostruzionePuntiDiInteressi(m_puntiDiInteresseSpawnatiNellArea,punto));
        for (int i = 0; i < punto.points.Count; i++)
        {
            ConnectedPoints connectedPoints = punto.points[i];
            for (int i1 = 0; i1 < connectedPoints.points.Count; i1++)
            {
                PuntoDiInteresse subPunto = connectedPoints.points[i1];
                if (subPunto == punto)
                {
#if UNITY_EDITOR
                    Debug.ClearDeveloperConsole();
                    Debug.LogError("Ricorsione infinita");
                    EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                    return;
                }
                if (subPunto == null) { return; }
                var room1 = SetRoom(subPunto, true);
                if (m_puntiDiInteresseSpawnatiNellArea.ContainsKey(subPunto)) { return; }
                m_puntiDiInteresseSpawnatiNellArea.Add(subPunto, room1);
                Publisher.Publish(new CostruzionePuntiDiInteressi(m_puntiDiInteresseSpawnatiNellArea,subPunto));
                CostruisciPuntoDiInteresse(subPunto, level + 1); // Chiamata ricorsiva
            }
        }
    }
    // public void CostruisciTuttiIPuntiDiInteresseDellArea()
    // {
    //     if (currentArea.puntiDiInteresse.Count <= 0) { return; }
    //     for (int i = 0; i < currentArea.puntiDiInteresse.Count; i++)
    //     {
    //         if(currentArea.puntiDiInteresse[i].points.Count == 0) { continue;}
    //         for (int y = 0; y < currentArea.puntiDiInteresse[i].points.Count; y++)
    //         {
    //             currentPoint = currentArea.puntiDiInteresse[i];
    //         }
    //     }
    // }
    public void VaiAlPuntoPrecedente(PuntoDiInteresse puntoDiInteresse)
    {

    }
    public void VaiAlPuntoSuccessivo(PuntoDiInteresse puntoDiInteresse)
    {

    }

    // FUNZIONI STANZE
    /// <summary>
    /// La funzione serve a settare la stanza per questo punto, se checkFirstRoom è selezionato allora verrà
    /// fatto in modo di pescare, se possibile, una stanza che ha il booleano attivo first room
    /// </summary>
    /// <param name="checkFirstRoom"></param>
    private RoomData SetRoom(PuntoDiInteresse punto, bool checkFirstRoom)
    {
        RoomData roomDataInterno = null;
        m_stanzeAttraversate = m_stanzeAttraversate + 1;
        if(punto.listaDiStanzeUniche == null){return null; }
        List<RoomData> stanzeCachateDaSO = punto.listaDiStanzeUniche;
        stanzeCachateDaSO = stanzeCachateDaSO.OrderByDescending(x => x.prioritaStanza).ToList();
        // CONTROLLARE SE UNA STANZA HA IL FIRST ROOM SU TRUE
        List<RoomData> roomFinaliCheRispettanoLeRegole = new();
        foreach (var room in stanzeCachateDaSO)
        {
            if (CheckRequisiti(room))
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
            roomDataInterno = roomFinaliCheRispettanoLeRegole[temp];

        }
        else if (roomFinaliCheRispettanoLeRegole.Count == 1)
        {
            roomDataInterno = roomFinaliCheRispettanoLeRegole[0];
        }
        Debug.Log($"<color=#00FF00>STANZA SELEZIONATA</color>");
        print(roomDataInterno.name);

        foreach (var item in roomDataInterno.flagsOnEnter.requiredFlagList)
        {
            bool hasFlag = m_flagManager.CheckFlag(item.flagList);
            if (hasFlag)
                m_flagManager.SetFlag(roomDataInterno.flagsOnEnter.flagToSet);
        }
        return roomDataInterno;
    }
    /// <summary>
    /// Questa funzione ha di diverso che costruisce la stanza in base al punto corrente
    /// </summary>
    /// <param name="checkFirstRoom"></param>
    /// <returns></returns>
    private IEnumerator SetRoom(bool checkFirstRoom)
    {
        m_stanzeAttraversate = m_stanzeAttraversate + 1;
        if(currentPoint.listaDiStanzeUniche == null || currentPoint == null){yield return null; }
        List<RoomData> stanzeCachateDaSO = currentPoint.listaDiStanzeUniche;
        stanzeCachateDaSO = stanzeCachateDaSO.OrderByDescending(x => x.prioritaStanza).ToList();
        // CONTROLLARE SE UNA STANZA HA IL FIRST ROOM SU TRUE
        List<RoomData> roomFinaliCheRispettanoLeRegole = new();
        foreach (var room in stanzeCachateDaSO)
        {
            if (CheckRequisiti(room))
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
        else if (roomFinaliCheRispettanoLeRegole.Count == 1)
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
        yield return null;
    }

    #region  PER REQUISITI MOSTRI E STANZE
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
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate > requisiti.valoreQuantitaUstioni.quantitaUstioni;
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate >= requisiti.valoreQuantitaUstioni.quantitaUstioni;
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate == requisiti.valoreQuantitaUstioni.quantitaUstioni;
                Debug.Log(roomCanSpawn);
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate < requisiti.valoreQuantitaUstioni.quantitaUstioni;
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate <= requisiti.valoreQuantitaUstioni.quantitaUstioni;
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
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate > set.valoreQuantitaUstioni.quantitaUstioni;
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate >= set.valoreQuantitaUstioni.quantitaUstioni;
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate == set.valoreQuantitaUstioni.quantitaUstioni;
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate < set.valoreQuantitaUstioni.quantitaUstioni;
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().ustioniAccumulate <= set.valoreQuantitaUstioni.quantitaUstioni;
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
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().maxHealth > requisiti.valoreQuantitaVitaMassima.quantita;
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().maxHealth >= requisiti.valoreQuantitaVitaMassima.quantita;
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().maxHealth == requisiti.valoreQuantitaVitaMassima.quantita;
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().maxHealth < requisiti.valoreQuantitaVitaMassima.quantita;
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().maxHealth <= requisiti.valoreQuantitaVitaMassima.quantita;
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
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().maxHealth > set.valoreQuantitaMassima.quantita;
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().maxHealth >= set.valoreQuantitaMassima.quantita;
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().maxHealth == set.valoreQuantitaMassima.quantita;
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().maxHealth < set.valoreQuantitaMassima.quantita;
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = m_playerControllerInstance.GetComponent<Damageable>().maxHealth <= set.valoreQuantitaMassima.quantita;
                break;
        }
        return roomCanSpawn;
    }



    private bool ControlloPercentualeVitaPlayer(RequisitiStanza requisiti)
    {
        bool roomCanSpawn = false;
        switch (requisiti.valoreVitaRimasta.operatori)
        {
            case OperatoriDiComparamento.Maggiore:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth * 100) > requisiti.valoreVitaRimasta.percentuale;
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth * 100) >= requisiti.valoreVitaRimasta.percentuale;
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth * 100) == requisiti.valoreVitaRimasta.percentuale;
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth * 100) < requisiti.valoreVitaRimasta.percentuale;
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth * 100) <= requisiti.valoreVitaRimasta.percentuale;
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
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth * 100) > set.valoreVitaRimasta.percentuale;
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth * 100) >= set.valoreVitaRimasta.percentuale;
                break;
            case OperatoriDiComparamento.Uguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth * 100) == set.valoreVitaRimasta.percentuale;
                break;
            case OperatoriDiComparamento.Minore:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth * 100) < set.valoreVitaRimasta.percentuale;
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                roomCanSpawn = (m_playerControllerInstance.GetComponent<Damageable>().currentHealth / m_playerControllerInstance.GetComponent<Damageable>().maxHealth * 100) <= set.valoreVitaRimasta.percentuale;
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
        // Ritorna false se il tipo di statistica non � valido
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
        // Ritorna false se il tipo di statistica non � valido
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
                temp = requisiti.valoreCorruzione.valoreDaComparare > m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione;
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                temp = requisiti.valoreCorruzione.valoreDaComparare >= m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione;
                break;
            case OperatoriDiComparamento.Uguale:
                temp = requisiti.valoreCorruzione.valoreDaComparare == m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione;
                break;
            case OperatoriDiComparamento.Minore:
                temp = requisiti.valoreCorruzione.valoreDaComparare < m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione;
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                temp = requisiti.valoreCorruzione.valoreDaComparare <= m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione;
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
                temp = set.valoreCorruzione.valoreDaComparare > m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione;
                break;
            case OperatoriDiComparamento.MaggioreOUguale:
                temp = set.valoreCorruzione.valoreDaComparare >= m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione;
                break;
            case OperatoriDiComparamento.Uguale:
                temp = set.valoreCorruzione.valoreDaComparare == m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione;
                break;
            case OperatoriDiComparamento.Minore:
                temp = set.valoreCorruzione.valoreDaComparare < m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione;
                break;
            case OperatoriDiComparamento.MinoreOUguale:
                temp = set.valoreCorruzione.valoreDaComparare <= m_playerControllerInstance.GetComponent<MalattiaHandler>().currentCorruzione;
                break;
            default:
                break;
        }
        return temp;
    }



    #endregion









    // ROOMS
    public void SpawnCurrentRoom()
    {
        try
        {
            GameObject roomSpawned = Instantiate(currentRoom.prefabStanza);
            Publisher.Publish(new OnRoomSpawnedMessage());
            m_surface = roomSpawned.GetComponentInChildren<NavMeshSurface>();
            GameObject playerSpawnato = SetPlayerInRoom(roomSpawned);
            AppManager.Instance.SetPlayerObject(playerSpawnato);
            m_playerControllerInstance = playerSpawnato.GetComponent<PlayerController>();
            AppManager.Instance.SetCameraPlayer();
            if ((currentRoom.tipiDiStanza & TipiDiStanzaFLag.Combattimento) != 0)
            {
                List<EnemySet> validEnemyList = new List<EnemySet>();
                validEnemyList = GetValidEnemySet(currentRoom.setDiMostriDellaStanza);
                SetEnemySet(validEnemyList);
                Debug.Log($"<b><color=#00ff00ff>ENEMY SET FINALE: {currentEnemySet}</color></b>");
                StartCoroutine(StartOndate());
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

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
        if (enemySetValidiAiRequisiti.Count == 0)
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
        if (enemySetValidiAiRequisiti.Count == 0)
        {
            int rangeRandomPerSpwanForzatoSet = UnityEngine.Random.Range(0, currentArea.enemySet.Count);
            enemySetValidiAiRequisiti.Add(currentArea.enemySet[rangeRandomPerSpwanForzatoSet]);
            Debug.LogError("Zero enemy set validi, metti almeno un'enemy set valido.\n E stato scelto un set casuale dall'area in modo forzato, risolvere");
        }
        return enemySetValidiAiRequisiti;
    }
    public void SetEnemySet(List<EnemySet> enemySets)

    {
        if (enemySets.Count == 1)
        {
            currentEnemySet = enemySets[0];

        }
        else if (enemySets.Count > 1)
        {
            int pesoTotale = 0;
            for (int i = 0; i < enemySets.Count; i++)
            {
                pesoTotale += enemySets[i].pesoSet;
            }

            int randomValue = UnityEngine.Random.Range(0, pesoTotale);

            int pesoAccumulato = 0;
            for (int i = 0; i < enemySets.Count; i++)
            {
                float percentuale = (float)enemySets[i].pesoSet / pesoTotale;
                if (randomValue >= pesoAccumulato && randomValue < pesoAccumulato + enemySets[i].pesoSet)
                {
                    Debug.Log($"<b>Hai selezionato l'elemento</b>{i}<quad material=1 size=20 x=0.1 y=0.1 width=0.5 height=0.5> ");
                    currentEnemySet = enemySets[i];
                    break;
                }

                pesoAccumulato += enemySets[i].pesoSet;
            }

        }
    }


    private IEnumerator StartOndate()
    {
        yield return new WaitForSeconds(2f);
        GameManager.ChangeGameState(GameStates.Combattimento);
        EnemySet.Ondata ondata = new();
        if (m_indiceOndataCorrente + 1 <= currentEnemySet.listaDiOndate.Count)
        {
            ondata = currentEnemySet.listaDiOndate[m_indiceOndataCorrente];
        }
        else
        {
            yield return null;
        }
        float enemyQuantity = (int)UnityEngine.Random.Range(ondata.minEnemyOndata, ondata.maxEnemyOndata);
        m_enemyQuantity = (int)enemyQuantity;
        enemyQuantity = AppManager.Instance.controlloMalattiaManager.mortality.applyMortalityToEnemySet(m_enemyQuantity, currentEnemySet.tipologiaNemico);
        foreach (var enemy in ondata.mostri)
        {
            int quantitaDaTogliere = Mathf.RoundToInt(enemyQuantity * (enemy.percentualeSpawnMostri / 100));
            int quantitaTipoMostri = -quantitaDaTogliere;
            if (enemyQuantity <= 0) { break; }
            for (int y = 0; y < quantitaDaTogliere; y++)
            {
                Vector2 posizioneMostro = CalcolaPosizioneGiustaPerMostro(enemy.playerDistance);
                SpawnOrMoveEnemy(enemy.nemicoDaIstanziare, posizioneMostro);
            }
        }
    }

    private void SpawnOrMoveEnemy(AssetReferenceGameObject nemicoDaIstanziare, Vector2 posizioneMostro)
    {
        string tipoDiNemicoDaIstanziare = nemicoDaIstanziare.RuntimeKey.ToString(); // Ottieni l'identificatore univoco come stringa
        GameObject nemico = GetInactiveEnemyFromPool(tipoDiNemicoDaIstanziare);
        if (nemico != null)
        {
            if (!nemico.activeSelf)
                nemico.SetActive(true);
            nemico.transform.position = posizioneMostro;
        }
        else
        {
            LoadAndInstantiateAddressable(nemicoDaIstanziare, posizioneMostro);
        }
    }

    private GameObject GetInactiveEnemyFromPool(string tipoDiNemicoDaIstanziare)
    {
        foreach (var enemy in AppManager.Instance.enemyManager.activeEnemyList)
        {
            if (!enemy.activeSelf && tipoDiNemicoDaIstanziare.Equals(enemy.GetComponent<EnemyController>().ID))
            {
                AppManager.Instance.enemyManager.AddEnemyToLists(enemy);
                return enemy;
            }
        }
        return null;
    }

    private async void LoadAndInstantiateAddressable(AssetReferenceGameObject nemicoDaIstanziare, Vector2 posizioneMostro)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(nemicoDaIstanziare);
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject nemico = Instantiate(handle.Result, posizioneMostro, Quaternion.identity);
            nemico.GetComponent<EnemyController>().ID = nemicoDaIstanziare.RuntimeKey.ToString(); // Assegna l'identificatore univoco
            AppManager.Instance.enemyManager.AddEnemyToLists(nemico);
        }
        else
        {
            Debug.LogError("Failed to load addressable: " + nemicoDaIstanziare);
        }
        Addressables.Release(handle);
    }

    public void VaiAvantiDiOndata()
    {
        if (currentEnemySet != null)
        {

            if (currentEnemySet.listaDiOndate.Count > m_indiceOndataCorrente + 1)
            {
                m_indiceOndataCorrente++;
                StartCoroutine(StartOndate());
            }
            else
            {
                StartCoroutine(DichiaraFineCombattimento());
            }
        }
        else
        {
            Debug.LogError("ERRORE: currentEnemySet == null");
        }
    }



    public void VaiIndietroOndate()
    {
        if (m_indiceOndataCorrente == 0) { return; }
        m_indiceOndataCorrente--;
        StartCoroutine(StartOndate());
    }


    /// <summary>
    /// Aspetta x secondi prima di dichiarare il cambio di stato
    /// </summary>
    /// <returns></returns>
    private IEnumerator DichiaraFineCombattimento()
    {
        m_enemyQuantity = 0;
        GameManager.ChangeGameState(GameStates.FineCombattimento);
        AppManager.Instance.enemyManager.RemoveEveryEnemyFromScene();
        yield return null;
    }









    private Vector2 CalcolaPosizioneGiustaPerMostro(EDistanzaPlayerDaNemico playerDistance)
    {

        Vector2 spawnPosition = Vector2.zero;

        Bounds navMeshBounds = m_surface.navMeshData.sourceBounds;
        Vector2 roomSize = new Vector2(navMeshBounds.size.x, navMeshBounds.size.z);

        float rangeMin = 0f;
        float rangeMax = 0f;

        switch (playerDistance)
        {
            case EDistanzaPlayerDaNemico.NonSpecificato:
                rangeMin = 1;
                rangeMax = 1;
                break;
            case EDistanzaPlayerDaNemico.Vicino:
                rangeMin = UnityEngine.Random.Range(0.0f, 0.3f);
                rangeMax = UnityEngine.Random.Range(rangeMin, 0.3f);
                break;
            case EDistanzaPlayerDaNemico.Intermedia:
                rangeMin = UnityEngine.Random.Range(0.3f, 0.7f);
                rangeMax = UnityEngine.Random.Range(rangeMin, 0.7f);
                break;
            case EDistanzaPlayerDaNemico.Lontano:
                rangeMin = UnityEngine.Random.Range(0.7f, 1.0f);
                rangeMax = UnityEngine.Random.Range(rangeMin, 1.0f);
                break;
            default:
                break;
        }

        spawnPosition = FindValidSpawnPosition(rangeMin, rangeMax, roomSize);

        return spawnPosition;
    }

    private Vector2 FindValidSpawnPosition(float rangeMin, float rangeMax, Vector2 roomSize)
    {
        NavMeshHit hit;
        Vector2 spawnPosition = Vector2.zero;
        bool positionFound = false;

        Vector2 roomCenter = new Vector2(m_surface.navMeshData.sourceBounds.center.x, m_surface.navMeshData.sourceBounds.center.y);
        int maxAttempts = 200;

        for (int i = 0; i < maxAttempts; i++)
        {
            var randomPosition = new Vector2(
            UnityEngine.Random.Range(m_surface.navMeshData.sourceBounds.min.x * rangeMin, m_surface.navMeshData.sourceBounds.max.x * rangeMax),
            UnityEngine.Random.Range(m_surface.navMeshData.sourceBounds.min.z * rangeMin, m_surface.navMeshData.sourceBounds.max.z * rangeMax));
            spawnPosition = new Vector2(randomPosition.x, randomPosition.y);
            int walkableAreaMask = 1 << NavMesh.GetAreaFromName("Walkable");
            float distanceToPlayer = Vector2.Distance(spawnPosition, m_playerControllerInstance.gameObject.transform.position);
            if (Vector2.Distance(spawnPosition, m_playerControllerInstance.gameObject.transform.position) < 1)
            {
                continue;
            }
            if (!NavMesh.SamplePosition(spawnPosition, out hit, 1.0f, walkableAreaMask))
            {
                continue;
            }
            if (distanceToPlayer < rangeMin || distanceToPlayer > rangeMax)
            {
                continue;
            }
            bool tooCloseToEdges = Vector2.Distance(spawnPosition, roomCenter) < 1;
            bool tooCloseToOtherSpawns = IsTooCloseToOtherSpawns(spawnPosition);
            if (tooCloseToEdges || tooCloseToOtherSpawns)
                continue;

            positionFound = true;
            break;
        }

        Debug.Log("<color=blue>Posizione trovata: " + spawnPosition + "</color>");
        return spawnPosition;
    }

    private bool IsTooCloseToOtherSpawns(Vector2 spawnPosition)
    {
        // Implementa il codice per verificare se la posizione è troppo vicina ad altri punti di spawn
        // Ritorna true se è troppo vicina, altrimenti false
        bool notCloseToOtherSpawns = false;
        if (m_lastMonsterSpawnPosition == null)
        {
            m_lastMonsterSpawnPosition = spawnPosition;
        }
        else
        {
            if (Vector2.Distance(spawnPosition, m_lastMonsterSpawnPosition) < 1.5f)
            {
                notCloseToOtherSpawns = true;
            }
            else
            {
                m_lastMonsterSpawnPosition = spawnPosition;
            }
        }

        return notCloseToOtherSpawns;
    }




    private GameObject SetPlayerInRoom(GameObject roomSpawned)
    {
        Room roomScript = roomSpawned.GetComponent<Room>();
        GameObject playerSpawned = new GameObject();
        int range = 0;
        if (roomScript != null)
        {
            if (roomScript.puntiDiSpawnPossibili.Count > 1)
            {
                range = UnityEngine.Random.Range(0, roomScript.puntiDiSpawnPossibili.Count);
                playerSpawned = Instantiate(AppManager.Instance.playePrefabReference, roomScript.puntiDiSpawnPossibili[range].localPosition, Quaternion.identity);
                print("<color=#00000>PLAYER SPAWNATO</color>");
            }
            else if (roomScript.puntiDiSpawnPossibili.Count == 1)
            {
                playerSpawned = Instantiate(AppManager.Instance.playePrefabReference, roomScript.puntiDiSpawnPossibili[0].localPosition, Quaternion.identity);
            }
            else if (roomScript.puntiDiSpawnPossibili.Count == 0 || roomScript.puntiDiSpawnPossibili == null)
            {
                Grid gridComponent = roomSpawned.GetComponent<Grid>();
                Tilemap tilemap = gridComponent.GetComponentInChildren<Tilemap>();

                if (tilemap != null)
                {
                    BoundsInt bounds = tilemap.cellBounds;
                    Vector3 cellSize = tilemap.cellSize;
                    Vector3 gridMin = tilemap.GetCellCenterWorld(bounds.min);
                    Vector3 gridMax = tilemap.GetCellCenterWorld(bounds.max);
                    float randomX = UnityEngine.Random.Range(gridMin.x, gridMax.x);
                    float randomY = UnityEngine.Random.Range(gridMin.y, gridMax.y);
                    Vector3 randomPointInTilemap = new Vector3(randomX, randomY, 0f);
                }

            }
        }
        return playerSpawned;
    }


}
