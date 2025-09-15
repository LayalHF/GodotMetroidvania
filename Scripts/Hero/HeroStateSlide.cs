using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroStateSlide : Timer, IHeroState
    {
        private HeroStateMachine Hero;
        private bool SlideTimerHasTimedOut = false;      // if the timer has timed out (stop sliding)
        private bool SlidingInitiated = false;
        private bool Initialized = false;
        public IHeroState DoState(HeroStateMachine hero, float deltatime)
        {
            InitState(hero);
            return Slide(deltatime);
        }

        public void InitState(HeroStateMachine hero)
        {
            if (!Initialized)
            {
                //Set references
                Hero = hero;
                ConnectSlideTimerSignal();
                Initialized = true;
            }
        }

        private void ConnectSlideTimerSignal()
        {
            // whenever the slide timer times out, run the OnSlideTimeout() method
            Hero.HeroTimers.SlideTimer.Connect("timeout", this, nameof(OnSlideTimerTimeout));
        }

        private void OnSlideTimerTimeout()
        {
            Hero.HeroTimers.SlideTimer.Stop();
            SlideTimerHasTimedOut = true;
        }
    
        private IHeroState Slide(float delta)
        {
            CheckSlideStateInitiated();
            Hero.HeroMoveLogic.EnableSnap();
            Hero.HeroAnimations.Play("HeroSlide");
            Hero.HeroCollisionShapes.ChangeCollisionShapesToSlide();

            if (!Hero.IsOnFloor())
            {
                SlidingInitiated = false;
                Hero.HeroMoveLogic.MovementDisabled = false;
                Hero.HeroCollisionShapes.ChangeCollisionShapesToStanding();
                return Hero.StateFall;
            }
        
            if (SlideTimerHasTimedOut)
            {
                SlidingInitiated = false;       // reset the initiated state to false
                return Hero.StateSlideStandUp;      // switch to the slide stand up state
            }
        
            return PerformSlide();
        }

        private void CheckSlideStateInitiated()
        {
            if (!SlidingInitiated)
            {
                SlideTimerHasTimedOut = false;       // reset tim,er
                Hero.HeroTimers.SlideTimer.Start();     //stat the slide timer
                SlidingInitiated = true;
            }
        }
    
        private IHeroState PerformSlide()
        {
            if (Hero.HeroAnimations.FlipH)
            {
                Hero.HeroMoveLogic.Velocity.x = -Hero.HeroMoveLogic.MaxMovementSpeed;

                // disable movement so the player should not be able to change direction
                Hero.HeroMoveLogic.MovementDisabled = true;
            }

            if (!Hero.HeroAnimations.FlipH)
            {
                Hero.HeroMoveLogic.Velocity.x = Hero.HeroMoveLogic.MaxMovementSpeed;
                Hero.HeroMoveLogic.MovementDisabled = true;
            }

            return Hero.StateSlide;
        }
    }
}
