using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkWrapAround : MonoBehaviour
{
    public GameObject OppositeBookend;
    GameObject parentBookEnd;

    // Start is called before the first frame update
    void Start()
    {
        parentBookEnd = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Transform playerTransform = null;

        // TODO collide player directly? For now use sensor, which has a boxcollider2d
        if(other.name == "Sensor") {
            playerTransform = other.transform.parent;
        } else if(other.GetComponent<Player>()) {
            playerTransform = other.transform;
        }

        if(playerTransform != null) {
            Vector3 positionRelativeToParentBookend = parentBookEnd.transform.InverseTransformPoint(playerTransform.position);
            Vector3 positionAtOppositeBookend = OppositeBookend.transform.TransformPoint(positionRelativeToParentBookend);

            CharacterController controller = playerTransform.GetComponent<CharacterController>();
            if(controller) controller.enabled = false;
            playerTransform.position = positionAtOppositeBookend;
            if(controller) controller.enabled = true;
            
        }
    }
}
