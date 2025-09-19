using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
    public class Hero2DRayCast
    {
        private HeroStateMachine Hero;
        private bool RaycastsInitialized = false;       // flag to keep track of if the raycast have been properly initialized
        public RayCast2D LedgeGrabRayCastTileAbove;     // The raycast to check if a tile is above the hero
        public RayCast2D LedgeGrabRayCastTileHead;      // The raycast to check if a tile is next to the head
        public RayCast2D LeftWallRayCast;     // The raycast to check if there is a wall to the left
        public RayCast2D RightWallRayCast;      // The raycast to check if there is a wall to the right

        public Hero2DRayCast(HeroStateMachine hero, ref bool initOk)
        {
            Hero = hero;
            initOk = InitHeroRaycast();
        }

        private bool InitHeroRaycast()
        {
            RaycastsInitialized = true;
            LedgeGrabRayCastTileAbove = GetRaycastNode("LedgeGrabRayCastTileAbove");
            if (!RaycastsInitialized)
            {
                return false;
            }
            LedgeGrabRayCastTileHead = GetRaycastNode("LedgeGrabRayCastTileHead");
            if (!RaycastsInitialized)
            {
                return false;
            }
            
            LeftWallRayCast = GetRaycastNode("LeftWallRayCast");
            if (!RaycastsInitialized)
            {
                return false;
            }
            
            RightWallRayCast = GetRaycastNode("RightWallRayCast");
            if (!RaycastsInitialized)
            {
                return false;
            }
            return true;
        }
        
        private RayCast2D GetRaycastNode(string rayCastNodeName)
        {
            string raycastPath = "./RayCasts/" + rayCastNodeName;       // set path to the raycast
            var raycast = Hero.GetNode<RayCast2D>(raycastPath);     // try to get the raycast node

            if(raycast == null)
            {
                RaycastsInitialized = false;
                
                GD.PrintErr("[Hero2DRayCast] - GetRaycastNode() Could not initialize raycast, node: '" + rayCastNodeName + "' was not found!");
            }
            // return raycast node even if it's null
            return raycast;
        }
    }
}
