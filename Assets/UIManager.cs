using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour, ISubscriber
{
    [Header("Progress Bar Malattia")]
    public MeshMaskUI MalattiaProgressBar;
    public TextMeshProUGUI levelMalattia;
    [Header("Progress Bar Guarigione")]
    public MeshMaskUI GuarigioneProgressBar;
    public TextMeshProUGUI levelGuarigione;
    [Header("Corruzione")]
    public MeshMaskUI CorruptionBar;
    public TextMeshProUGUI percTextCorruption;
    [Header("Malattia")]
    public MeshMaskUI IllBar;
    public TextMeshProUGUI percTextIll;
    [Header("HealthBar")]
    public MeshMaskUI HealthBar;
    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;

    // Start is called before the first frame update
    void Awake()
    {
        Publisher.Subscribe(this, new UpdateUiBar());
        Publisher.Subscribe(this, new UpdateHealthBar());
    }

    private void OnDisable()
    {
        Publisher.Unsubscribe(this, new UpdateUiBar());
        Publisher.Unsubscribe(this, new UpdateHealthBar());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPublish(IMessage message)
    {
        if (message is UpdateUiBar)
        {
            updateUiBar(message);
        }
        else if (message is UpdateHealthBar)
        {
            updateHealthBar(message);
        }
    }

    private void updateHealthBar(IMessage message)
    {
        UpdateHealthBar mess = (UpdateHealthBar)message;
        if (HealthBar != null)
        {
            HealthBar.setPercentage(mess.percentageBar);
        }
        else
        {
            Debug.LogError("UI non collegata");
        }
        if (maxHealthText != null)
        {
            maxHealthText.text = mess.maxHealth;
        }
        else
        {
            Debug.LogError("UI non collegata");
        }
        if (currentHealthText != null)
        {
            currentHealthText.text = mess.currentHealth;
        }
        else
        {
            Debug.LogError("UI non collegata");
        }

    }

    private void updateUiBar(IMessage message)
    {
        UpdateUiBar mess = (UpdateUiBar)message;
        switch (mess.typeBar)
        {
            case UpdateUiBar.barType.MalattiaProgressBar:
                if (MalattiaProgressBar != null)
                {
                    MalattiaProgressBar.setPercentage(mess.percentageBar);
                }
                else
                {
                    Debug.LogError("UI non collegata");
                }
                if (levelMalattia != null)
                {
                    levelMalattia.text = mess.textBar;
                }
                else
                {
                    Debug.LogError("UI non collegata");
                }
                break;
            case UpdateUiBar.barType.GuarigioneProgressBar:
                if (GuarigioneProgressBar != null)
                {
                    GuarigioneProgressBar.setPercentage(mess.percentageBar);
                }
                else
                {
                    Debug.LogError("UI non collegata");
                }
                if (levelGuarigione != null)
                {
                    levelGuarigione.text = mess.textBar;
                }
                else
                {
                    Debug.LogError("UI non collegata");
                }
                break;
            case UpdateUiBar.barType.MalattiaBar:
                if (IllBar != null)
                {
                    IllBar.setPercentage(mess.percentageBar);
                }
                else
                {
                    Debug.LogError("UI non collegata");
                }
                if (percTextIll != null)
                {
                    percTextIll.text = mess.textBar + "%";
                }
                else
                {
                    Debug.LogError("UI non collegata");
                }

                break;
            case UpdateUiBar.barType.CorruzioneBar:
                if (CorruptionBar != null)
                {
                    CorruptionBar.setPercentage(mess.percentageBar);
                }
                else
                {
                    Debug.LogError("UI non collegata");
                }
                if (percTextCorruption != null)
                {
                    percTextCorruption.text = mess.textBar + "%";
                }
                else
                {
                    Debug.LogError("UI non collegata");
                }
                break;
            default:
                break;
        }
    }
}
