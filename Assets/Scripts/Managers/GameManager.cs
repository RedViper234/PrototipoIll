using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : Manager,ISubscriber
{
    private static GameStates m_statiDiGioco;
    [SerializeField, MyReadOnly] private GameStates m_statoDiGioco;
    private void Start() {
        Publisher.Subscribe(this,new GameStateChangedMessage());
    }
    public static GameStates GetCurrentGameState(){
        return m_statiDiGioco;
    }
    public static void ChangeGameState(GameStates state){
        if(m_statiDiGioco == state) return;
        m_statiDiGioco = state;
        Publisher.Publish(new GameStateChangedMessage(state));
        Debug.Log("<color=purple>Changing game state to " + state+"</color>");
    }

    public void OnPublish(IMessage message)
    {
        if(message is GameStateChangedMessage){
            GameStateChangedMessage temp = (GameStateChangedMessage)message;
            m_statiDiGioco = temp.state;
        }
    }

    private void OnDisable() {
        Publisher.Unsubscribe(this,new GameStateChangedMessage());
    }

}
public enum GameStates{
    MenuPrincipale,
    PreRun,
    Combattimento,
    CombattimentoBoss,
    FineCombattimento,
    Morte
}

public struct GameStateChangedMessage: IMessage{
    public GameStates state;
    public GameStateChangedMessage(GameStates state) => this.state = state;
}