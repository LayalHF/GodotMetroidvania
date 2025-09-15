using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
	public class HeroStateSliderStandUp : Timer, IHeroState
	{
		private bool IsInNarrowPassage;
		private bool Initialized = false;
		private bool SlideStandUpTimerHasTimedout = false;     // bool to keep track of if the slide stand up timer has timed out
		private bool SlidingStandUpInitiated = false;
		private HeroStateMachine Hero;

		public IHeroState DoState(HeroStateMachine hero, float deltatime)
		{
			InitState(hero);
			return SlideStandUp(deltatime);
		}

		private void InitState(HeroStateMachine hero)
		{
			// If the slide stand up class has not been initialized
			if (!Initialized)
			{
				Hero = hero;
				ConnectSlideStandUpTimerSignal();
				Initialized = true;
			}
		}

		private void ConnectSlideStandUpTimerSignal()
		{
			Hero.HeroTimers.SlideStandUpTimer.Connect("timeout", this, nameof(OnSlideStandUpTimerTimeout));
		}

		private void OnSlideStandUpTimerTimeout()
		{
			Hero.HeroTimers.SlideStandUpTimer.Stop();
			SlideStandUpTimerHasTimedout = true;
		}
		
		private IHeroState SlideStandUp(float delta)
		{
			CheckSlideStandUpStateInitiated();
			CheckIfInNarrowPassage();     // check if the hero is in a narrow passage
			if (Hero.IsOnFloor() || IsInNarrowPassage)
			{
				Hero.HeroCollisionShapes.ChangeCollisionShapesToSlideStandUp();
				Hero.HeroAnimations.Play("HeroSlideStandUp");
				// if the slide stand up timer has timed out
				if (SlideStandUpTimerHasTimedout)
				{
					Hero.HeroMoveLogic.MovementDisabled = false;    // enable movement
					
					// if player pressing the shift key
					if (Input.IsActionJustPressed("Slide"))
					{
						ResetSlideStandUpState();
						Hero.HeroCollisionShapes.ChangeCollisionShapesToSlide();
						return Hero.StateSlide;
					}

					if (!IsInNarrowPassage)
					{
						ResetSlideStandUpState();
						Hero.HeroCollisionShapes.ChangeCollisionShapesToStanding();
						return Hero.StateIdle;
					}
				}
			}
			return Hero.StateSlideStandUp;
		}

		private void ResetSlideStandUpState()
		{
			SlideStandUpTimerHasTimedout = false;
			SlidingStandUpInitiated =  false;
		}
		private void CheckSlideStandUpStateInitiated()
		{
			if (!SlidingStandUpInitiated)
			{
				Hero.HeroTimers.SlideStandUpTimer.Start();
				SlidingStandUpInitiated = true;
			}
		}
		private void CheckIfInNarrowPassage()
		{
			// If the head is colliding 
			if (Hero.HeroCollisionShapes.IsCollisionShape2DColliding("CollisionShapeHead"))
			{
				IsInNarrowPassage = true;     // flag that the hero is in a narrow passage
			}
			else
			{
				IsInNarrowPassage = false;		// flag that the hero is not in a narrow passage
			}
		}
	}
}
