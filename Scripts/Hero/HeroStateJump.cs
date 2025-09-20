using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroStateJump : IHeroState
    {
        private const float CutJumpThreshold = -200.0f; // the threshold for cutting a jump start
        private const float JumpForceAfterJumpCutShort = -320.0f; // the jump force after a jump has been cut short
        private int MaxJumps = 2;       // maximum number of jumps the hero can perform in-air
        private int JumpCount = 0;      // the number of jumps that has been performed in-air
        
        
        public IHeroState DoState(HeroStateMachine hero, float deltatime)
        {
            return Jump(hero, deltatime);
        }

        private IHeroState Jump(HeroStateMachine hero, float deltatime)
        {
            hero.HeroMoveLogic.DisableSnap();

            // Set the animation hero to run
            hero.HeroAnimations.Play("HeroJump");

            // Cute jump short if the jump button is released
            if (Input.IsActionJustReleased("Jump") && hero.HeroMoveLogic.Velocity.y < CutJumpThreshold)
            {
                // set the velocity to the new jump force for when a jump has been cut short
                hero.HeroMoveLogic.Velocity.y = JumpForceAfterJumpCutShort;
                // return the falling state
                return hero.StateFall;
            }
            // if the glide action is pressed
            if (Input.IsActionJustPressed("Glide"))
            {
                hero.HeroAnimations.Play("HeroFall");       // so it looks like the hero is holding the glider
                hero.HeroEquipment.Glider.OpenGlider();
                return hero.StateGlide;
            }
            
            // if the attack action is pressed
            if (Input.IsActionJustPressed("Attack"))
            {
                return hero.StateAttack;
            }
            
            // if hero is not on the ground or floor
            if (!hero.IsOnFloor())
            {
                // going upwards
                if (hero.HeroMoveLogic.Velocity.y < 0)
                {
                    // corner correct the jump if needed
                    CornerCorrectTheJump(hero, deltatime);
                    return hero.StateJump;
                }
                // falling
                if (hero.HeroMoveLogic.Velocity.y > 0)
                {
                    return hero.StateFall;
                }
            }
            else if (hero.IsOnFloor())
            {
                ResetJumpCounter();
                return hero.StateIdle;
            }

            return hero.StateJump;
        }

        public void SetMaxJump(int numJumps)
        {
            MaxJumps = numJumps;       // set the new number of max jumps
        }
        
        public void ResetJumpCounter()
        {
            JumpCount = 0;     // reset jump counter to 0
        }

        public bool CanJumpAgainInAir()
        {
            JumpCount++;

            if (JumpCount < MaxJumps)
            {
                return true;
            }
            
            return false;
        }

        public bool CanWallJump(HeroStateMachine hero)
        {
            // if there is a wall left to the player and the hero is facing the right side(non-flipped sprite)
            if (hero.HeroRaycasts.LeftWallRayCast.IsColliding() && !hero.HeroAnimations.FlipH)
            {
                return true;        // return true, the hero can wall jump
            }
            // if there is a wall right to the player and the hero is facing the left side(flipped sprite)
            if (hero.HeroRaycasts.RightWallRayCast.IsColliding() && hero.HeroAnimations.FlipH)
            {
                return true;
            }
            return false;
        }

        public bool CanHeroJumpBufferJump(HeroStateMachine hero)
        {
            // if hero is not on the floor & the jump buffer raycast is colliding & the hero is falling
            if (!hero.IsOnFloor() && hero.HeroRaycasts.JumpBufferRayCast.IsColliding() &&
                hero.HeroMoveLogic.Velocity.y > 0)
            {
                return true;
            }
            return false;
        }

        public void CornerCorrectTheJump(HeroStateMachine hero, float delta)
        {
            // if the left raycast is colliding, but not the middle one
            if (hero.HeroRaycasts.CornerCorrectionLeftRayCast.IsColliding()
                && !hero.HeroRaycasts.CornerCorrectionMiddleRayCast.IsColliding())
            {  
                // if the hero is not next to the wall - checked by making sure that the head ray is not colliding
                if (!hero.HeroRaycasts.LedgeGrabRayCastTileHead.IsColliding())
                {
                    hero.Translate(new Vector2(400*delta,0));
                }
            }
            // if the right raycast is colliding, but not the middle one
            if (hero.HeroRaycasts.CornerCorrectionRightRayCast.IsColliding()
                && !hero.HeroRaycasts.CornerCorrectionMiddleRayCast.IsColliding())
            {
                if (!hero.HeroRaycasts.LedgeGrabRayCastTileHead.IsColliding())
                {
                    hero.Translate(new Vector2(-400*delta,0));
                }
            }
            
            
        }
    }
}