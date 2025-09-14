using Godot;
using System;
using MetroidvaniaProject.Scripts.Hero;

public class HeroTimers
{
   private HeroStateMachine Hero;
   public Timer SlideTimer;
   public Timer SlideStandUpTimer;
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

      if (!TimersInitialized) return false;

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
