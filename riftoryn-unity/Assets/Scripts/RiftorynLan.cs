using System;
using System.Collections;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

// Small two-player LAN transport. Match simulation stays in RiftorynTrial on the host.
public sealed class RiftorynLan : MonoBehaviour
{
    const ushort Port = 7777;
    const string Message = "riftoryn-lan-v1";
    NetworkManager manager;
    RiftorynTrial trial;
    ulong peer = ulong.MaxValue;
    bool closing, cleaning;
    public bool Active => cleaning || (manager && manager.IsListening);
    public bool IsHost => manager && manager.IsListening && manager.IsHost;
    public bool Connected => Active && peer != ulong.MaxValue;
    public string Status { get; private set; } = "";
    public event Action<string> Received;
    public event Action PeerConnected;
    public event Action PeerDisconnected;

    public void StartHost(RiftorynTrial owner) => StartNetwork(owner, true, "127.0.0.1");
    public void StartClient(RiftorynTrial owner, string address) => StartNetwork(owner, false, address);
    void StartNetwork(RiftorynTrial owner, bool host, string address)
    {
        if (cleaning || manager) { Status = "Zamykanie poprzedniego połączenia, spróbuj ponownie za chwilę."; return; }
        trial = owner; closing = false;
        var root = new GameObject("LAN NetworkManager");
        var transport = root.AddComponent<UnityTransport>();
        transport.SetConnectionData(address.Trim(), Port, host ? "0.0.0.0" : null);
        manager = root.AddComponent<NetworkManager>();
        manager.NetworkConfig = new NetworkConfig { NetworkTransport = transport, ConnectionApproval = true, EnableSceneManagement = false };
        if (host) manager.ConnectionApprovalCallback = (request, response) => {
            response.Approved = peer == ulong.MaxValue && manager.ConnectedClientsIds.Count < 2;
            response.CreatePlayerObject = false;
            response.Pending = false;
        };
        manager.OnClientConnectedCallback += OnConnected;
        manager.OnClientDisconnectCallback += OnDisconnected;
        bool ok = host ? manager.StartHost() : manager.StartClient();
        if (!ok) { Status = "Nie udało się uruchomić połączenia."; Stop(); return; }
        manager.CustomMessagingManager.RegisterNamedMessageHandler(Message, OnMessage);
        Status = host ? "Oczekiwanie na drugiego gracza (port UDP 7777)." : "Łączenie z gospodarzem...";
    }
    void OnConnected(ulong id)
    {
        if (!manager || closing) return;
        if (manager.IsHost) { if (id == manager.LocalClientId) return; peer = id; Status = "Gracz połączony."; }
        else { if (id != manager.LocalClientId) return; peer = NetworkManager.ServerClientId; Status = "Połączono z gospodarzem."; PeerConnected?.Invoke(); }
    }
    void OnDisconnected(ulong id)
    {
        if (closing || !manager) return;
        if ((manager.IsHost && id == peer) || (!manager.IsHost && id == manager.LocalClientId)) {
            peer = ulong.MaxValue; Status = "Połączenie zostało przerwane."; PeerDisconnected?.Invoke();
        }
    }
    void OnMessage(ulong sender, FastBufferReader reader)
    {
        if (!manager || sender != peer) return;
        reader.ReadValueSafe(out string value);
        Received?.Invoke(value);
    }
    public void Send(string value, bool reliable = false)
    {
        if (!Connected || manager.CustomMessagingManager == null) return;
        using var writer = new FastBufferWriter(8192, Allocator.Temp);
        writer.WriteValueSafe(value);
        manager.CustomMessagingManager.SendNamedMessage(Message, peer, writer, reliable ? NetworkDelivery.ReliableFragmentedSequenced : NetworkDelivery.UnreliableSequenced);
    }
    public void Stop()
    {
        closing = true; peer = ulong.MaxValue;
        if (!manager) return;
        var old = manager; manager = null;
        if (old.IsListening) old.Shutdown();
        if (isActiveAndEnabled) StartCoroutine(FinishShutdown(old));
    }
    IEnumerator FinishShutdown(NetworkManager old)
    {
        cleaning = true;
        yield return null;
        while (old && old.ShutdownInProgress) yield return null;
        if (old) Destroy(old.gameObject);
        cleaning = false;
    }
    void OnDestroy() => Stop();
}
