using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour,ISubscriber
{
    public List<Transform> puntiDiSpawnPossibili;

    private NavMeshSurface m_surface;

    private void Awake()
    {
        Publisher.Subscribe(this, new OnRoomSpawnedMessage());
        m_surface = GetComponentInChildren<NavMeshSurface>();
    }
    public void OnPublish(IMessage message)
    {
        if(message is OnRoomSpawnedMessage)
        {
            m_surface.BuildNavMesh();
        }
    }
    private void OnDisable() =>Publisher.Unsubscribe(this, new OnRoomSpawnedMessage());
    private void OnDestroy() =>Publisher.Unsubscribe(this, new OnRoomSpawnedMessage());
}
