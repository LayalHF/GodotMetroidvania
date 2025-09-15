using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroStateMachine : KinematicBody2D
    {
        public HeroStateIdle StateIdle = new HeroStateIdle(); // The Idle state
        public HeroStateRun StateRun = new HeroStateRun(); // The Run state
        public HeroStateFall StateFall = new HeroStateFall(); // the Fall state
        public HeroStateJump StateJump = new HeroStateJump(); // the Jump state
        public HeroStateInitJump StateInitJump = new HeroStateInitJump(); // the Jump state
        public HeroStateSlide StateSlide = new HeroStateSlide(); // the slide state
        public HeroStateSliderStandUp StateSlideStandUp = new HeroStateSliderStandUp(); // the slide stand up state

        public HeroMoveLogic HeroMoveLogic;
        public HeroCollisionShapes HeroCollisionShapes;
        public HeroTimers HeroTimers;
        public AnimatedSprite HeroAnimations; // The Hero Animations
        private IHeroState CurrentState; // The current state the Hero is in
        private bool IsInitialized = false; // Boolean to keep track of if the state machine is properly initialized
        public bool IsMoving = false; // to keep track if the Hero is moving

        public string LastPlayedHeroAnimation = string.Empty;

        // Called when node enters the scene tree for the first time
        public override void _Ready()
        {
            IsInitialized = InitHeroStateMachine();

            if (!IsInitialized)
            {
                GD.PrintErr("[HeroStateMachine] - HeroStateMachine has not been initialized.");
            }
        }

        private bool InitHeroStateMachine()
        {
            var initOk = true; // bool to keep track of in the state machine was initialized ok
            CurrentState = StateIdle; // Set the starting state to idle
            HeroMoveLogic = new HeroMoveLogic(this); // Initialize Hero Move Logic
            
            HeroCollisionShapes = new HeroCollisionShapes(this, ref initOk);
            if (!initOk)
            {
                return false;
            }

            HeroTimers = new HeroTimers(this, ref initOk);
            if (!initOk)
            {
                return false;
            }
            
            initOk = GetHeroAnimationsNode(); //Get the hero animations node
            return initOk;
        }

        private bool GetHeroAnimationsNode()
        {
            // Get the animation node of the Hero
            HeroAnimations = GetNode<AnimatedSprite>("./HeroAnimations");

            // if node could not be found
            if (HeroAnimations is null)
            {
                GD.PrintErr("[HeroStateMachine] - GetHeroAnimationsNode() HeroAnimations is null.");
                return false;
            }

            return true;
        }

        private void UpdateHeroState(float delta)
        {
            // Update the current state
            CurrentState = CurrentState.DoState(this, delta);
        }

        public override void _PhysicsProcess(float delta)
        {
            // only run the logic if the hero state machine is properly initialized
            if (IsInitialized)
            {
                UpdateHeroState(delta);
                HeroMoveLogic.ApplyGravity(delta);
                HeroMoveLogic.UpdateMovement(delta);
                HeroMoveLogic.MoveHero(delta);
            }
        }

        private void HeroAnimationDone()
        {
            // The animation_finished signal just called this method
            // set the lastplayedanimation to the animation name that just finished playing
            LastPlayedHeroAnimation = HeroAnimations.Animation.ToString();
        }
    }
}