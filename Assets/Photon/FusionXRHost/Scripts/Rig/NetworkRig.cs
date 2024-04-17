using Fusion.XR.Host.Grabbing;
using UnityEngine;


namespace Fusion.XR.Host.Rig
{
    /**
     * 
     * Networked VR user
     * 
     * Handle the synchronisation of the various rig parts: headset, left hand, right hand, and playarea (represented here by the NetworkRig)
     * Use the local HardwareRig rig parts position info when this network rig is associated with the local user 
     * 
     * 
     **/

    [RequireComponent(typeof(NetworkTransform))]
    // We ensure to run after the NetworkTransform or NetworkRigidbody, to be able to override the interpolation target behavior in Render()
    [DefaultExecutionOrder(NetworkRig.EXECUTION_ORDER)]
    public class NetworkRig : NetworkBehaviour
    {
        public const int EXECUTION_ORDER = 100;
        public HardwareRig hardwareRig;
        public NetworkHand leftHand;
        public NetworkHand rightHand;
        public NetworkHeadset headset;
        public NetworkGrabber leftGrabber;
        public NetworkGrabber rightGrabber;

        [HideInInspector]
        public NetworkTransform networkTransform;

        private void Awake()
        {
            networkTransform = GetComponent<NetworkTransform>();
            if (leftHand != null) leftGrabber = leftHand.GetComponent<NetworkGrabber>();
            if (rightHand != null) rightGrabber = rightHand.GetComponent<NetworkGrabber>();
        }

        // As we are in host topology, we use the input authority to track which player is the local user
        public bool IsLocalNetworkRig => Object.HasInputAuthority;

        public override void Spawned()
        {
            base.Spawned();
            if (IsLocalNetworkRig)
            {
                hardwareRig = FindObjectOfType<HardwareRig>();
                if (hardwareRig == null) Debug.LogError("Missing HardwareRig in the scene");
            }
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            // update the rig at each network tick
            if (GetInput<RigInput>(out var input))
            {
                transform.position = input.playAreaPosition;
                transform.rotation = input.playAreaRotation;
                if (leftHand != null) leftHand.transform.position = input.leftHandPosition;
                if (leftHand != null) leftHand.transform.rotation = input.leftHandRotation;
                if (rightHand != null) rightHand.transform.position = input.rightHandPosition;
                if (rightHand != null) rightHand.transform.rotation = input.rightHandRotation;
                if (headset != null) headset.transform.position = input.headsetPosition;
                if (headset != null) headset.transform.rotation = input.headsetRotation;
                // we update the hand pose info. It will trigger on network hands OnHandCommandChange on all clients, and update the hand representation accordingly
                if (leftHand != null) leftHand.HandCommand = input.leftHandCommand;
                if (rightHand != null) rightHand.HandCommand = input.rightHandCommand;

                if (leftGrabber != null) leftGrabber.GrabInfo = input.leftGrabInfo;
                if (rightGrabber != null) rightGrabber.GrabInfo = input.rightGrabInfo;
            }
        }

        public override void Render()
        {
            base.Render();
            if (IsLocalNetworkRig)
            {
                // Extrapolate for local user:
                // we want to have the visual at the good position as soon as possible, so we force the visuals to follow the most fresh hardware positions
                // To update the visual object, and not the actual networked position, we move the interpolation targets
                transform.position = hardwareRig.transform.position;
                transform.rotation = hardwareRig.transform.rotation;
                if (leftHand != null) leftHand.transform.position = hardwareRig.leftHand.transform.position;
                if (leftHand != null) leftHand.transform.rotation = hardwareRig.leftHand.transform.rotation;
                if (rightHand != null) rightHand.transform.position = hardwareRig.rightHand.transform.position;
                if (rightHand != null) rightHand.transform.rotation = hardwareRig.rightHand.transform.rotation;
                if (headset != null) headset.transform.position = (hardwareRig.headset != null) ? hardwareRig.headset.transform.position : hardwareRig.transform.position;
                if (headset != null) headset.transform.rotation = (hardwareRig.headset != null) ? hardwareRig.headset.transform.rotation : hardwareRig.transform.rotation;
            }
        }
    }
}
