using UnityEngine;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerDropDown : MonoBehaviour
    {
    
     
        public string oneWayPlatformLayerName = "OneWayPlatform";
        public string playerLayerName = "Player";



        // Update is called once per frame
        void Update()
        {
            // drop down script
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayerName),
                LayerMask.NameToLayer(oneWayPlatformLayerName), InputManager.instance.moveInput.y < 0);

        }
    }
}
