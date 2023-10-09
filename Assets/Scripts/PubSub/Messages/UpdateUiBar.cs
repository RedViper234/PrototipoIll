using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UpdateUiBar : IMessage
{
    public string textBar;
    public float percentageBar;
    public barType typeBar;
    public enum barType
    {
        MalattiaProgressBar,
        GuarigioneProgressBar,
        MalattiaBar,
        CorruzioneBar,
        MalattiaMultiplier,
    }

    public UpdateUiBar(string textBar, float percentageBar, barType typeBar)
    {
        this.textBar = textBar;
        this.percentageBar = percentageBar;
        this.typeBar = typeBar;
    }
}
