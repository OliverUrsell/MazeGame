using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterMovementHelper : CharacterControllerDriver
{
    void FixedUpdate()
    {
        UpdateCharacterController();
    }
}
