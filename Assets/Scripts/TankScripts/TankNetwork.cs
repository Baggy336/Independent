using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TankNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<TankNetValue> netState = new(writePerm: NetworkVariableWritePermission.Owner);
    private Vector3 velocity;
    private float rotation;
    [SerializeField] private float interpolationTime = 0.1f;

    // Set up network variables to be passed through the serializer
    struct TankNetValue : INetworkSerializable
    {
        private float xPos, zPos;
        private float yRot;

        internal Vector3 Pos
        {
            get => new Vector3(xPos, 0, zPos);
            set
            {
                xPos = value.x;
                zPos = value.z;
            }
        }

        internal Vector3 Rot
        {
            get => new Vector3(0, yRot, 0);
            set => yRot = value.y;

        }
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref xPos);
            serializer.SerializeValue(ref zPos);

            serializer.SerializeValue(ref yRot);
        }
    }

    // Get the current network values for the owner, and for the other tanks in the game, set their position and rotation
    private void Update()
    {
        if (IsOwner)
        {
            netState.Value = new TankNetValue()
            {
                Pos = transform.position,
                Rot = transform.rotation.eulerAngles
            };
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, netState.Value.Pos, ref velocity, interpolationTime);
            transform.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, netState.Value.Rot.y, ref rotation, interpolationTime), 0);
        }
    }
}
