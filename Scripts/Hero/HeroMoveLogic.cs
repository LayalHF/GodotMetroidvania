using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroMoveLogic
    {
        private float Gravity = 1500; //Gravity Strength pulling the Hero towards the ground
        private float MovementAccelerations = 20; // The movement acceleration of the Hero
        public float MaxMovementSpeed = 200; // The max movement speed of the Hero
        private float Friction = 1.0f; // Friction for the Horizontal movement on the ground
        public Vector2 SnapVector;
        public Vector2 Velocity = Vector2.Zero; // The direction the Hero is moving in
        private HeroStateMachine Hero;
        public bool MovementDisabled = false;
        public bool GravityDisabled = false;
        public HeroMoveLogic(HeroStateMachine hero)
        {
            Hero = hero;
        }

        public void MoveHero(float delta)
        {
            // Move the Hero according to controller input
            Velocity = Hero.MoveAndSlideWithSnap(Velocity, SnapVector, Vector2.Up, stopOnSlope: true);

            if (!Hero.IsMoving)
            {
                if (IsHeroOnSlope() || Hero.IsOnFloor())
                {
                    Velocity.x = Mathf.Lerp(Velocity.x, 0, Friction);
                }
            }
        }

        public void UpdateMovement(float delta)
        {
            float leftDirectionStrength = Input.GetActionStrength("MoveLeft");
            float rightDirectionStrength = Input.GetActionStrength("MoveRight");
            
            if (!MovementDisabled)
            {
                UpdateVelocity(leftDirectionStrength, rightDirectionStrength);

                //Update left & right movement
                UpdateRightMovement(leftDirectionStrength, rightDirectionStrength);
                UpdateLeftMovement(leftDirectionStrength, rightDirectionStrength);

                // update the isMoving State
                UpdateIsMoving(leftDirectionStrength, rightDirectionStrength);
            }
        }

        private void UpdateVelocity(float leftDirectionStrength, float rightDirectionStrength)
        {
            if (leftDirectionStrength > 0 && rightDirectionStrength > 0)
            {
                // if the hero is facing left
                if (Hero.HeroAnimations.FlipH)
                {
                    Velocity.x -= leftDirectionStrength * MovementAccelerations;
                }
                else
                {
                    Velocity.x += rightDirectionStrength * MovementAccelerations;
                }
            }
            else
            {
                // update velocity accordingly
                Velocity.x += (rightDirectionStrength - leftDirectionStrength) * MovementAccelerations;
            }
            // Clamp the velocity to the maximum movement speed
            Velocity.x = Mathf.Clamp(Velocity.x, -MaxMovementSpeed, MaxMovementSpeed);
        }
        
        private void UpdateRightMovement(float leftDirectionStrength, float rightDirectionStrength)
        {
            // if Hero is moving to the right
            if (leftDirectionStrength < rightDirectionStrength)
            {
                if (IsHeroOnSlope())
                {
                    // if friction is max
                    if (Friction == 1.0f)
                    {
                        // run at full speed at all times
                        Velocity.x = MaxMovementSpeed;
                    }
                    // draw the hero animations unflipped
               
                }
                Hero.HeroAnimations.FlipH = false;
                
                // reset the ledge-grab ray casts
                Hero.HeroRaycasts.LedgeGrabRayCastTileAbove.RotationDegrees = 0;
                Hero.HeroRaycasts.LedgeGrabRayCastTileHead.RotationDegrees = 0;
            }
        }
    
        private void UpdateLeftMovement(float leftDirectionStrength, float rightDirectionStrength)
        {
            // if Hero is moving to the right
            if (rightDirectionStrength < leftDirectionStrength)
            {
                if (IsHeroOnSlope())
                {
                    // if friction is max
                    if (Friction == 1.0f)
                    {
                        // run at full speed at all times
                        Velocity.x = -MaxMovementSpeed;
                    }
                    // draw the hero animations unflipped
               
                }
                Hero.HeroAnimations.FlipH = true;
                
                // reset the ledge-grab ray casts
                Hero.HeroRaycasts.LedgeGrabRayCastTileAbove.RotationDegrees = -180;
                Hero.HeroRaycasts.LedgeGrabRayCastTileHead.RotationDegrees = -180;
            }
        }

        private void UpdateIsMoving(float leftDirectionStrength, float rightDirectionStrength)
        {
            // if there is no movement in the left or the right directions
            if (leftDirectionStrength is 0 && rightDirectionStrength is 0)
            {
                Hero.IsMoving = false;
            }
            else
            {
                Hero.IsMoving = true;
            }
        }

        private bool IsHeroOnSlope()
        {
            // if the floor normal x variable is not zero (horizontal)
            if (Hero.GetFloorNormal().x != 0)
            {
                // the hero is standing on a slope
                return true;
            }
            // the hero is not standing on the slope
            return false;
        }

        public void ApplyGravity(float delta)
        {
            if (!GravityDisabled)
            {
                // apply gravity to the hero
                Velocity.y += Gravity * delta;
            }
        }
            public void EnableSnap()
            {
                SnapVector = new Vector2(0, 15);
            }
        
            public void DisableSnap()
            {
                SnapVector = Vector2.Zero;
            }
    }

}