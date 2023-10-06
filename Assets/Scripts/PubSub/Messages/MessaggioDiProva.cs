using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MessaggioDiProva : IMessage
{
    public string prova;
	public MessaggioDiProva(string prova)
	{
		this.prova = prova;
	}
}
