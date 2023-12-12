using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using AYellowpaper.SerializedCollections;

public class MapManager : Manager, ISubscriber
{
    private PlayerInput inputActions;
    [HideInInspector] public GameObject backGroundMap;
    [SerializeField] private GameObject prefabPuntoDiInteresse;
    [SerializeField] private GameObject lineRendererObject;



    private UILineRenderer m_lineRenderer;
    private bool isOpen = false;
    private bool m_isFirstPoint = false;
    private Vector2 lastPosition = new Vector2(0, 0);
    private int m_counter = 0;

    private Dictionary<PuntoDiInteresse, StrutturaPerDictionaryRoom> m_puntiDiInteresseSpawnatiNellArea = new();

    public void OnPublish(IMessage message)
    {
        if (message is OpenMapMessage)
        {
            OpenMapMessage temp = (OpenMapMessage)message;
        }
        else if (message is CostruzionePuntiDiInteressi)
        {
            CostruzionePuntiDiInteressi costruzionePuntiDiInteressi = (CostruzionePuntiDiInteressi)message;
            m_puntiDiInteresseSpawnatiNellArea = costruzionePuntiDiInteressi.dizionarioPuntiEStanze;
            CostruisciPuntoUI(costruzionePuntiDiInteressi.puntoDiInteresseChiave);
        }
        else if (message is PassGameObjectMessage)
        {
            PassGameObjectMessage passGameObjectMessage = (PassGameObjectMessage)message;
            if (passGameObjectMessage.objectToPass.GetComponent<MapUI>() != null)
            {
                backGroundMap = passGameObjectMessage.objectToPass;
            }
        }

    }

    public void Awake()
    {
        inputActions = new PlayerInput();
    }
    private IEnumerator Start()
    {
        Publisher.Publish(new OpenMapMessage(false));
        m_lineRenderer = Instantiate(lineRendererObject, backGroundMap.transform).GetComponent<UILineRenderer>();
        yield return new WaitUntil(() => Screen.width > 0 && Screen.height > 0);
        m_lineRenderer.SetGridSize(new Vector2Int(Screen.width, Screen.height));
    }


    public void CostruisciPuntoUI(PuntoDiInteresse punto)
    {
        try
        {
            GameObject puntoIstanziato;
            Debug.Log("CostruisciPuntoUI");
            RoomData roomData = m_puntiDiInteresseSpawnatiNellArea[punto].room;
            if (roomData != null)
            {
                if (prefabPuntoDiInteresse == null)
                {
                    Debug.LogError("PuntoDiInteresse non trovato");
                    return;
                }
                Vector2 position;
                Color puntoColor = Color.white;
                if (m_isFirstPoint)
                {
                    position = CalcolaPosizioneCasuale();

                }
                else
                {
                    position = backGroundMap.transform.GetChild(0).GetComponent<RectTransform>().position;
                    Destroy(backGroundMap.transform.GetChild(0).gameObject);
                    m_isFirstPoint = true;

                }
                switch (roomData.tipiDiStanza)
                {
                    case TipiDiStanzaFlag.Boss:
                        puntoColor = Color.red;
                        break;
                    case TipiDiStanzaFlag.Combattimento:
                        puntoColor = Color.cyan;
                        break;
                    case TipiDiStanzaFlag.Storia:
                        puntoColor = Color.green;
                        break;
                    case TipiDiStanzaFlag.Evento:
                        puntoColor = Color.yellow;
                        break;

                    default:
                        break;
                }
                puntoIstanziato = InstantiatePuntoDiInteresse(position, puntoColor, $"{punto.name}\n {roomData.tipiDiStanza}");
                puntoIstanziato.transform.SetParent(backGroundMap.transform);
                lastPosition = position;
                m_lineRenderer.SetPoints(puntoIstanziato.transform.position);
                m_counter++;
            }
            else
            {
                
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private GameObject InstantiatePuntoDiInteresse(Vector2 position, Color color, string nomeDescrizione)
    {
        GameObject puntoIstanziato = Instantiate(prefabPuntoDiInteresse, position, Quaternion.identity);
        puntoIstanziato.GetComponent<PuntoDiInteresseComponent>().SetDescrizione(nomeDescrizione);
        puntoIstanziato.GetComponent<Image>().color = color;

        return puntoIstanziato;
    }

    private Vector2 CalcolaPosizioneCasuale()
    {
        System.Random random = new System.Random();
        int n = random.Next(0, 1);
        bool isMinus = n == 1 ? true : false;
        int xPosRandom = 0;
        int yPosRandom = 0;
        if (isMinus)
        {
            xPosRandom = random.Next(-150, -200);
            yPosRandom = random.Next(0, -200);
        }
        else
        {
            xPosRandom = random.Next(150, 200);
            yPosRandom = random.Next(0, 200);
        }

        return lastPosition += new Vector2(xPosRandom, yPosRandom);
    }



    public void OpenMap(bool isOpen)
    {
        Publisher.Publish(new OpenMapMessage(isOpen));
        backGroundMap.GetComponent<Image>().enabled = isOpen;
        Image[] array1 = backGroundMap.GetComponentsInChildren<Image>();
        for (int i = 0; i < array1.Length; i++)
        {
            Image item = array1[i];
            item.enabled = isOpen;
        }
        TextMeshProUGUI[] array = backGroundMap.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < array.Length; i++)
        {
            TextMeshProUGUI item = array[i];
            item.enabled = isOpen;
        }
        backGroundMap.GetComponentInChildren<UILineRenderer>().enabled = isOpen;
    }
    private void OpenMapViaCodice(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isOpen = !isOpen;
            OpenMap(isOpen);
            Time.timeScale = isOpen ? 0 : 1;
        }
    }



    private void OnEnable()
    {
        Publisher.Subscribe(this, new OpenMapMessage());
        Publisher.Subscribe(this, new CostruzionePuntiDiInteressi());
        Publisher.Subscribe(this, new PassGameObjectMessage());
        inputActions.UI.Enable();
        inputActions.UI.OpenMap.performed += OpenMapViaCodice;

    }
    private void OnDisable()
    {
        Publisher.Unsubscribe(this, new OpenMapMessage());
        Publisher.Unsubscribe(this, new CostruzionePuntiDiInteressi());
        inputActions.UI.Disable();
        inputActions.UI.OpenMap.performed -= OpenMapViaCodice;

    }



}