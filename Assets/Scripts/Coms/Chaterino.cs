using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine.EventSystems;

public class Chaterino : MonoBehaviour, IChatClientListener
{
    [SerializeField] private ChatClient _chatClient;
    [SerializeField] private TextMeshProUGUI _chatContent;
    [SerializeField] private TMP_InputField _chatInput;
    [SerializeField] private string _channel;
    [SerializeField] private KeyCode _chatBind = KeyCode.Return;
    [SerializeField] private bool _enabledInput = false;

    [Header("Commands")]
    [SerializeField] private string _commandObstacles = "/easy";
    [SerializeField] private string _commandTeleport = "/tp";
    [SerializeField] private string _commandCloseGame = "/terminate";
    [SerializeField] private string _commandKick = "/kick";
    [SerializeField] private string _command1 = "/1";
    [SerializeField] private string _command2 = "/2";





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

        // Sending a Whisper Command.
        if (messageWords.Length > 2 && isCommand)
        {
            CheckCommand(messageWords);
        }

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
                DisableInput();
            }
        }
    }

    private void EnableInput()
    {
        _enabledInput = true;
        _chatInput.gameObject.SetActive(true);
    }

    private void DisableInput()
    {
        _enabledInput = false;
        _chatInput.gameObject.SetActive(false);
    }

    private void CheckCommand(string[] messageWords)
    {
        if (messageWords[0] == _commandTeleport)
        {
            // Teleport to Specified Player.
        } 

        else if (messageWords[0] == _commandObstacles)
        {
            // Destroy All Obstacles in Map.
        }

        else if (messageWords[0] == _commandCloseGame)
        {
            // Close Game Lobby.
        }

        else if (messageWords[0] == _commandKick)
        {
            // Kick Specified Player.
        }

        else if (messageWords[0] == _command1)
        {

        }

        else if (messageWords[0] == _command2)
        {

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
    private void CommandTeleport(/*Player*/)
    {

    }

    /// <summary>
    /// Command for Disabling Obstacles.
    /// </summary>
    private void CommandObstacles()
    {

    }

    /// <summary>
    /// Closes the Lobby for Everyone.
    /// </summary>
    private void CommandCloseGame()
    {

    }

    /// <summary>
    /// Kicks a Specified Player.
    /// /kick {username}
    /// </summary>
    private void CommandKick(/*Player*/)
    {

    }

    /// <summary>
    /// Extra Command 1
    /// </summary>
    private void Command1()
    {

    }

    /// <summary>
    /// Extra Command 2
    /// </summary>
    private void Command2()
    {

    }
}
