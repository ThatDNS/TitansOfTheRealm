using Fusion;
using TMPro;
using UnityEngine;

public class Warrior : NetworkBehaviour
{
    private NetworkCharacterController networkCC;
    public float speed = 5.0f;

    private bool saidHello = false;
    private TMP_Text txtMessage;

    private void Awake()
    {
        networkCC = GetComponent<NetworkCharacterController>();
        saidHello = false;

        txtMessage = FindObjectOfType<TMP_Text>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            //data.direction.Normalize();
            //networkCC.Move(speed * data.direction * Runner.DeltaTime);
        }
    }

    private void Update()
    {
        if (Object.HasInputAuthority && !saidHello)
        {
            //RPC_SendMessage("Hello!");
            saidHello = true;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        RPC_RelayMessage(message, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayMessage(string message, PlayerRef messageSource)
    {
        if (txtMessage == null)
            txtMessage = FindObjectOfType<TMP_Text>();

        if (messageSource == Runner.LocalPlayer)
        {
            message = $"You: {message}\n";
        }
        else
        {
            message = $"Warrior: {message}\n";
        }

        txtMessage.text += message;
    }

}
