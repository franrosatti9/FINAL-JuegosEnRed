using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MasterManager : MonoBehaviourPunCallbacks
{
    public GameManager gameManager;
    Dictionary<Player, CharacterModel> _dicChars = new Dictionary<Player, CharacterModel>();
    Dictionary<CharacterModel, Player> _dicPlayer = new Dictionary<CharacterModel, Player>();

    [SerializeField] List<GameObject> obstacles = new List<GameObject>(); 
    static MasterManager _instance;
    public static MasterManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            DebugDictionaries();
        }
    }
    [PunRPC]
    public void RequestConnectPlayer(Player client, string characterPrefab, Transform spawn)
    {
        var obj = PhotonNetwork.Instantiate(characterPrefab, spawn.position, Quaternion.identity);
        var character = obj.GetComponent<CharacterModel>();
        photonView.RPC("UpdatePlayer", RpcTarget.All, client, character.photonView.ViewID);
    }

    [PunRPC]
    public void RequestAddPlayer(Player client, int id)
    {
        photonView.RPC("UpdatePlayer", RpcTarget.All, client, id);
    }

    [PunRPC]
    public void UpdatePlayer(Player client, int id)
    {
        //WaitToUpdatePlayer(client, id);
        PhotonView pv = PhotonView.Find(id);
        var character = pv.gameObject.GetComponent<CharacterModel>();
        _dicChars[client] = character;
        _dicPlayer[character] = client;
        //gameManager.SetManager(character);
    }

    public void RPCMaster(string name, params object[] p)
    {
        RPC(name, PhotonNetwork.MasterClient, p);
    }
    public void RPC(string name, Player target, params object[] p)
    {
        photonView.RPC(name, target, p);
    }
    public void RPC(string name, RpcTarget target, params object[] p)
    {
        photonView.RPC(name, target, p);
    }

    public void RemoveModel(CharacterModel model)
    {
        if (_dicPlayer.ContainsKey(model))
        {
            var player = _dicPlayer[model];
            photonView.RPC("RequestRemovePlayer", RpcTarget.All, player);
        }
    }
    public void RemovePlayer(Player player)
    {
        photonView.RPC("RequestRemovePlayer", RpcTarget.All, player);
    }

    [PunRPC]
    public void RequestProjectile(Player client)
    {
        if (_dicChars.ContainsKey(client))
        {
            var character = _dicChars[client];
            character.Attack();
        }
    }
    [PunRPC]
    public void RequestRemovePlayer(Player client)
    {
        if (_dicChars.ContainsKey(client))
        {
            var character = _dicChars[client];
            _dicChars.Remove(client);
            if (character != null)
                _dicPlayer.Remove(character);
        }
    }

    public Player GetClientFromModel(CharacterModel model)
    {
        if (_dicPlayer.ContainsKey(model))
        {
            return _dicPlayer[model];
        }
        return null;
    }

    public CharacterModel GetModelFromClient(Player client)
    {
        if (_dicChars.ContainsKey(client))
        {
            return _dicChars[client];
        }
        return null;
    }

    public void DebugDictionaries()
    {
        Debug.Log("?");
        foreach(var kvp in _dicChars)
        {
            Debug.Log($"{kvp.Key.NickName}: {kvp.Value.gameObject.name}");
        }
            Debug.Log(_dicChars.Count);
    }

    IEnumerator WaitToUpdatePlayer(Player client, int id)
    {
        yield return new WaitForSeconds(0.2f);
        PhotonView pv = PhotonView.Find(id);
        var character = pv.gameObject.GetComponent<CharacterModel>();
        _dicChars[client] = character;
        _dicPlayer[character] = client;
    }

    [PunRPC]
    public void DeactivateObstacles()
    {
        for(int i = 0; i < obstacles.Count; i++)
        {
            obstacles[i].SetActive(false);
        }
    }

    [PunRPC]
    public void ForceDisconnect()
    {
        gameManager.QuitRoom();
    }
}
