using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game.ViewTools
{
    public class Camera
    {
        public Vector2f Position;

        public FloatRect ViewPort;

        public float Rotation { get; set; }

        public float Zoom { get; set; } = 1;

        private View view;

        public Camera(FloatRect configuration)
        {
            Position = new Vector2f(Configuration.Width / 2, Configuration.Height / 2);

            this.view = new View(new FloatRect(
                new Vector2f(0, 0),
                new Vector2f(Configuration.Width, Configuration.Height)));

            this.ViewPort = configuration;
        }

        public void Update(float deltaT)
        {
            // If we dont have camera movement enabled, return immediately
            if (!Configuration.AllowCameraMovement)
            {
                return;
            }

            // Camera movement is handled via an offset from the current position for simplicity.
            var ratio = Configuration.Width / (float)Configuration.Height;
            var offset = new Vector2f(0, 0);

            // Translation
            if (Keyboard.IsKeyPressed(Configuration.PanUp))
                offset.Y -= Configuration.CameraMovementSpeed * deltaT;

            if (Keyboard.IsKeyPressed(Configuration.PanLeft))
                offset.X -= Configuration.CameraMovementSpeed * deltaT / ratio;

            if (Keyboard.IsKeyPressed(Configuration.PanDown))
                offset.Y += Configuration.CameraMovementSpeed * deltaT;

            if (Keyboard.IsKeyPressed(Configuration.PanRight))
                offset.X += Configuration.CameraMovementSpeed * deltaT / ratio;

            // Zoom
            if (Keyboard.IsKeyPressed(Configuration.ZoomIn))
                this.Zoom += Configuration.CameraZoomSpeed * deltaT;
            else if (Keyboard.IsKeyPressed(Configuration.ZoomOut))
                this.Zoom -= Configuration.CameraZoomSpeed * deltaT;
            else
                this.Zoom = 1;

            // Rotation
            if (Keyboard.IsKeyPressed(Configuration.RotateLeft))
                this.Rotation -= Configuration.CameraRotationSpeed * deltaT;

            if (Keyboard.IsKeyPressed(Configuration.RotateRight))
                this.Rotation += Configuration.CameraRotationSpeed * deltaT;

            // Update all the things we just calculated.
            this.Position += offset;
            
            this.view.Rotation = this.Rotation;

            this.view.Viewport = this.ViewPort;

            this.SetCentre(this.Position, 0.2f);

            this.view.Zoom(this.Zoom);
        }

        public void SetCentre(Vector2f centre, float proportion = 1)
        {
            var difference = (this.view.Center - centre) * proportion;

            this.view.Center = this.view.Center -= difference;
        }

        public void ScaleToWindow(float width, float height)
        {
            var viewAspect = GetDesiredAspectRatio();
            var windowAspect = width / height;

            this.ViewPort = new FloatRect(0, 0, 1, 1);
            if(windowAspect > viewAspect)
            {
                this.ViewPort.Width = viewAspect / windowAspect;
                this.ViewPort.Left = (1f - this.ViewPort.Width) / 2f;
            }
            else
            {
                this.ViewPort.Height = windowAspect / viewAspect;
                this.ViewPort.Top = (1 - this.ViewPort.Height) / 2f;
            }
        }

        private float GetDesiredAspectRatio()
        {
            return Configuration.Width / (float)Configuration.Height;
        }

        public View GetView()
        {
            return view;
        }
    }
}
