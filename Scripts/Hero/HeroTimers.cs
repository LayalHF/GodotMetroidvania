using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
   public class HeroTimers
   {
      private HeroStateMachine Hero;
      
      public Timer SlideTimer;     
      
      public Timer SlideStandUpTimer;     // stand up timer - used to switch to the idle state
      
      public Timer LedgeFallTimer;     // time that must pass before the ledge-grabbing again
      
      public Timer LedgeClimbTimer;    // hero ledge climb timer - timer for how long it takes for hero to climb a ledge
      
      public Timer AttackTimer;    // timer for standard attack durations
      
      public Timer CoyoteTimeTimer;    // timer for coyote time duration

      private bool TimersInitialized;     // flag to keep track of if the timers have been properly initialized

      public HeroTimers(HeroStateMachine hero, ref bool initOk)
      {
         Hero = hero;
         initOk = InitHeroTimers();
      }

      private bool InitHeroTimers()
      {
         TimersInitialized = true;
         SlideTimer = GetTimerNode("SlideTimer");
         if(!TimersInitialized)
         {
            return false;
         }

         SlideStandUpTimer = GetTimerNode("SlideStandUpTimer");
         if(!TimersInitialized)
         {
            return false;
         }
         
         LedgeFallTimer = GetTimerNode("LedgeFallTimer");
         if(!TimersInitialized)
         {
            return false;
         }
         
         LedgeClimbTimer = GetTimerNode("LedgeClimbTimer");
         if(!TimersInitialized)
         {
            return false;
         }
         
         AttackTimer = GetTimerNode("AttackTimer");
         if(!TimersInitialized)
         {
            return false;
         }
         
         CoyoteTimeTimer = GetTimerNode("CoyoteTimer");
         if(!TimersInitialized)
         {
            return false;
         }
         
         return true;
      }
   
      private Timer GetTimerNode(string timerNode)
      {
         string timerPath = "./Timers/" + timerNode;     // set the path to the timer
         var timer = Hero.GetNode<Timer>(timerPath);
         // if the timer was not found
         if (timer == null)
         {
            TimersInitialized = false;
         
            GD.PrintErr("[HeroTimers] - GetTimerNode() could nt initialize timer, node: '" + timerNode + "' was not found");
         }
         // return timer even if it is null
         return timer;
      }
   }
}
