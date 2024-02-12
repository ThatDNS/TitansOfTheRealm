using Fusion;

public class Warrior : NetworkBehaviour
{
    private NetworkCharacterController networkCC;
    public float speed = 5.0f;

    private void Awake()
    {
        networkCC = GetComponent<NetworkCharacterController>();        
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            networkCC.Move(speed * data.direction * Runner.DeltaTime);
        }
    }
}
