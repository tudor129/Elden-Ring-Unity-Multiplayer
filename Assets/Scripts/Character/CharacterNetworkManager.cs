using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterNetworkManager : NetworkBehaviour
{
    CharacterManager _character;
    
    [Header("Position")]
    public NetworkVariable<Vector3> _networkPosition = new NetworkVariable<Vector3>(
        Vector3.zero, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> _networkRotation = new NetworkVariable<Quaternion>(
        Quaternion.identity, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Owner);
    public Vector3 _networkPositionVelocity;
    public float _networkPositionSmoothTime = 0.1f;
    public float _networkRotationSmoothTime = 0.1f;
    
    [Header("Animator")]
    public NetworkVariable<float> _animatorHorizontalValue = new NetworkVariable<float>(
        0, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Owner);
    
    [FormerlySerializedAs("_animatorverticalValue")] public NetworkVariable<float> _animatorVerticalValue = new NetworkVariable<float>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> _networkMoveAmount = new NetworkVariable<float>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    [Header("Flags")]
    public NetworkVariable<bool> _isSprinting = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    [Header("Stats")]
    public NetworkVariable<int> _endurance = new NetworkVariable<int>(
        1,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> _currentStamina = new NetworkVariable<float>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    public NetworkVariable<int> _maxStamina = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    

    protected virtual void Awake()
    {
        _character = GetComponent<CharacterManager>();
    }
    
    // A SERVER RPC IS A FUNCTION CALLED FROM THE CLIENT, TO THE SERVER (IN OUR CASE THE HOST)
    [ServerRpc]
    public void NotifyServerOfAnActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion, ServerRpcParams serverRpcParams = default)
    {
        // Using the uLong clientID is considered a bad practice, but it is used here for simplicity
        // Using the serverRpcParams.Receive.SenderClientId is the recommended way to get the clientID
        var clientId = serverRpcParams.Receive.SenderClientId;
        // IF THIS CHARACTER IS THE HOST/SERVER, THEN ACTIVATE CLIENT RPC
        if (IsServer)
        {
            PlayActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
        }
    }
    
    // A CLIENT RPC IS A FUNCTION CALLED FROM THE SERVER, TO THE CLIENT (IN OUR CASE THE HOST)
    [ClientRpc]
    void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformActionAnimationFromServer(animationID, applyRootMotion);
        }
    }

    void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
    {
        _character._applyRootMotion = applyRootMotion;
        _character._animator.CrossFade(animationID, 0.2f);
    }
    
    
}
