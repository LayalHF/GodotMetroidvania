using System;
using Godot;

namespace MetroidvaniaProject.Scripts
{
    public class Glider : Node2D
    {
        private AnimatedSprite GliderAnimations; // the glider animation node
        public string LastPlayedAnimation = string.Empty; // name of the last glider animation that finished playing
        public bool FlipHorizontal = false; // if the glider should be drawn flipped horizontally

        public override void _Ready()
        {
            InitGlider(); // initialize the glider
        }

        private void InitGlider()
        {
            // get the glider animated sprite node
            GliderAnimations = GetNode<AnimatedSprite>("./AnimatedGlider");

            if (GliderAnimations == null)
            {
                GD.PrintErr("[Glider] - InitGlider() - Could not fine 'AnimatedGlider' node");
                return;
            }

            GliderAnimations.Connect("animation_finished", this, nameof(AnimationDone));
        }

        private void AnimationDone()
        {
            // update the last played animation
            LastPlayedAnimation = GliderAnimations.Animation;
        }

        public override void _Process(float delta)
        {
            GliderAnimations.FlipH = FlipHorizontal; // update if the glider should be drawn flipped

            // if the close animation just finished playing
            if (LastPlayedAnimation.Equals("GliderClose"))
            {
                Visible = false; // hide the glider
                LastPlayedAnimation = string.Empty;
            }

            // if open animation just finished playing
            if (LastPlayedAnimation.Equals("GliderOpen"))
            {
                Visible = true; 
                GliderAnimations.Play("Glide");
                LastPlayedAnimation = string.Empty;
            }
        }

        public void OpenGlider()
        {
            Visible =  true;
            GliderAnimations.Play("GliderOpen");
        }
        
        public void CloseGlider()
        {
            GliderAnimations.Play("GliderClose");
        }
    }
}