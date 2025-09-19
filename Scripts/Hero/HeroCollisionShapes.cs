using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroCollisionShapes
    {
        private HeroStateMachine Hero;
        private CollisionShape2D Head;
        private CollisionShape2D Body;
        private CollisionShape2D Slide;

        private bool ShapesInitialized;

        public HeroCollisionShapes(HeroStateMachine hero, ref bool initOk)
        {
            Hero = hero;
            initOk = InitHeroCollisionShapes();
        }
    
        private bool InitHeroCollisionShapes()
        {
            ShapesInitialized = true;

            Head = InitHeroCollisionShape("./CollisionShapeHead");
            if(!ShapesInitialized)
            {
                return false;
            }
        
            Body = InitHeroCollisionShape("./CollisionShapeBody");
            if(!ShapesInitialized)
            {
                return false;
            }
        
            Slide = InitHeroCollisionShape("./CollisionShapeSlide");
            if(!ShapesInitialized)
            {
                return false;
            }

            return ShapesInitialized;
        }
    
    
        private CollisionShape2D InitHeroCollisionShape(string shapeNodeName)
        {
            string collisionNodeName = "./" + shapeNodeName;
            var collisionShape = Hero.GetNode<CollisionShape2D>(collisionNodeName);
            if (collisionShape == null)
            {
                ShapesInitialized = false;
                GD.PrintErr("[HeroCollision] - InitHeroCollisionShape() could not initialize collisionShape, Noed: " + collisionNodeName + " was not found!");
            }
            return collisionShape;
        }

        public bool IsCollisionShape2DColliding(string collisionNodeName)
        {
            // loop through all the current collision shapes that are currently colliding
            for (int i = 0; i < Hero.GetSlideCount(); i++)
            {
                // get the collision
                var collision = Hero.GetSlideCollision(i);

                // if the collision is a collisionShape2D
                if (collision.LocalShape is CollisionShape2D)
                {
                    // Get the shape by typecasting it to a collisionShape2D
                    var shape = (CollisionShape2D)collision.LocalShape;
                
                    // if the names are equal, return true, the requested collision shape is colliding
                    if (shape.Name.Equals(collisionNodeName))
                        return true;
                }
            }

            return false;
        }

        public void ChangeCollisionShapesToSlide()
        {
            Head.Disabled = true;       // disable hero head collision shape
            Body.Disabled = true;       // disable hero body collision shape
            Slide.Disabled = false;     // enable hero head collision shape
        }
    
        public void ChangeCollisionShapesToStanding()
        {
            Head.Disabled = true;
            Body.Disabled = false;
            Slide.Disabled = true;
        }
    
        public void ChangeCollisionShapesToSlideStandUp()
        {
            Head.Disabled = false;
            Body.Disabled = true;
            Slide.Disabled = false;
        }
        
        public void DisableAllCollisionShapes()
        {
            Head.Disabled = true;
            Body.Disabled = true;
            Slide.Disabled = true;
        }
    }
}
