using System;

[Flags]
public enum TipiDiStanzaFlag
{
    None = 0,
    Combattimento = 2,
    Boss = 4,
    Evento = 8,
    Storia = 16,
}
public enum TipiDiStanza
{
    None,
    Combattimento,
    Boss,
    Evento,
    Storia,
}
