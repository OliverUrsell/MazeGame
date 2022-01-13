using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterMovementHelper : CharacterControllerDriver
{

    void FixedUpdate()
    {
        UpdateCharacterController();
        sendPosition();
    }

    class Test {
        public float x, y;
        public Test(float x, float y){
            this.x = x;
            this.y = y;
        }
    }

    void sendPosition(){
        Test position = new Test(transform.position.x, transform.position.z);
        // Debug.Log(System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(position)));
        HTTPClient.client.PutRequest("position", JsonUtility.ToJson(position));
    }
}
