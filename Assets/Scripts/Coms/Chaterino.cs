using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Chaterino : MonoBehaviour, IChatClientListener
{
    [SerializeField] private ChatClient _chatClient;
    [SerializeField] private TextMeshProUGUI _chatContent;
    [SerializeField] private TextMeshProUGUI _placeHolderText;
    [SerializeField] private TMP_InputField _chatInput;
    [SerializeField] private string _channel;
    [SerializeField] private KeyCode _chatBind = KeyCode.Return;
    [SerializeField] private bool _enabledInput = false;

    [Header("Commands")]
    [SerializeField] private string _commandObstacles = "/easy";
    [SerializeField] private string _commandTeleport = "/tp";
    [SerializeField] private string _commandCloseGame = "/terminate";
    [SerializeField] private string _commandKick = "/kick";
    [SerializeField] private string _commandSpeed = "/speed";
    [SerializeField] private string _commandFlipInput = "/hard";





    private void Start()
    {
        _chatClient = new ChatClient(this);
        var appID = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat;
        var appVersion = PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion;
        var auth = new AuthenticationValues(PhotonNetwork.NickName);
        _chatClient.Connect(appID, appVersion, auth);
    }

    private void Update()
    {
        _chatClient.Service();

        UpdateInput();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnDisconnected()
    {
        _chatContent.text += $"Could not Connect to {_channel}.\n";
    }

    public void OnConnected()
    {
        _channel = PhotonNetwork.CurrentRoom.Name;
        _chatClient.Subscribe(_channel);
        _chatContent.text += $"Connected to {_channel}.\n";
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            var currentUsername = senders[i];
            var currentMessage = messages[i];
            
            string color;
            if(PhotonNetwork.NickName == currentUsername) { color = "<color=yellow>"; }
            else { color = "<color=orange>"; }

            _chatContent.text += $"{color}{currentUsername}:</color> {currentMessage}\n";
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        string color = "<color=#FF00FF>";
        _chatContent.text += $"{color}{sender}:</color> {message}\n";
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }

    public void SendChatMessage()
    {
        var message = _chatInput.text;
        
        if(string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) return;;

        // Splits Input Text.
        string[] messageWords = message.Split(' ');
        bool isCommand = messageWords[0].StartsWith("/");
        Debug.Log(isCommand);

        // Sending a Command.
        if (isCommand)
        {
            CheckCommand(messageWords);
        }

        // Sending a chat message
        else
        {
            _chatClient.PublishMessage(_channel, message);
        }

        _chatInput.text = "";
    }

    private void UpdateInput()
    {
        if (Input.GetKeyDown(_chatBind))
        {
            if (_enabledInput == false)
            {
                EnableInput();
            }
            
            else if (_enabledInput == true)
            {
                SendChatMessage();
                DisableInput();
            }
        }
    }

    private void EnableInput()
    {
        _enabledInput = true;
        _chatInput.GetComponent<Image>().enabled = true;
        _chatInput.ActivateInputField();
        _chatInput.Select();
    }

    private void DisableInput()
    {
        _enabledInput = false;
        _chatInput.GetComponent<Image>().enabled = false;
        _chatInput.DeactivateInputField(true);
        _placeHolderText.text = "";
    }

    private void CheckCommand(string[] messageWords)
    {
        if (messageWords[0] == _commandTeleport)
        {
            // Teleport to Specified Player.
            CommandTeleport(messageWords[1]);
        } 

        else if (messageWords[0] == _commandObstacles)
        {
            // Destroy All Obstacles in Map.
            if (!PhotonNetwork.IsMasterClient) return;
            CommandObstacles();
        }

        else if (messageWords[0] == _commandCloseGame)
        {
            // Close Game Lobby.
            if (!PhotonNetwork.IsMasterClient) return;
            CommandCloseGame();
        }

        else if (messageWords[0] == _commandKick)
        {
            // Kick Specified Player.
            if (!PhotonNetwork.IsMasterClient) return;
            CommandKick(messageWords[1]);
        }

        else if (messageWords[0] == _commandSpeed)
        {
            int speed = int.Parse(messageWords[1]);
            CommandSpeed(speed);
        }

        else if (messageWords[0] == _commandFlipInput)
        {
            CommandHard();
        }

        else
        {
            Debug.LogWarning("Command Not Found.");
        }
    }

    /// <summary>
    /// Command for Teleporting to a Player.
    /// /tp {username}
    /// </summary>
    private void CommandTeleport(string player)
    {
        foreach(var client in PhotonNetwork.PlayerList)
        {
            if (client.NickName == player)
            {
                MasterManager.Instance.GetModelFromClient(PhotonNetwork.LocalPlayer).transform.position = MasterManager.Instance.GetModelFromClient(client).transform.position;
                return;
            }
        }
    }

    /// <summary>
    /// Command for Disabling Obstacles.
    /// </summary>
    private void CommandObstacles()
    {
        MasterManager.Instance.RPC("DeactivateObstacles", RpcTarget.All);
    }

    /// <summary>
    /// Closes the Lobby for Everyone.
    /// </summary>
    private void CommandCloseGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        MasterManager.Instance.gameManager.CloseRoom();
    }

    /// <summary>
    /// Kicks a Specified Player.
    /// /kick {username}
    /// </summary>
    private void CommandKick(string player)
    {
        foreach (var client in PhotonNetwork.PlayerList)
        {
            if (client.NickName == player)
            {
                MasterManager.Instance.RPC("ForceDisconnect", client);
                return;
            }
        }
    }

    /// <summary>
    /// Extra Command 1
    /// </summary>
    private void CommandSpeed(int newSpeed)
    {
        if (MasterManager.Instance.GetModelFromClient(PhotonNetwork.LocalPlayer) == null) return;
        MasterManager.Instance.GetModelFromClient(PhotonNetwork.LocalPlayer).speed = newSpeed;
    }

    /// <summary>
    /// Extra Command 2
    /// </summary>
    private void CommandHard()
    {
        if (MasterManager.Instance.GetModelFromClient(PhotonNetwork.LocalPlayer) == null) return;
        MasterManager.Instance.GetModelFromClient(PhotonNetwork.LocalPlayer).GetComponent<CharacterController>().FlipInput();
    }
}
