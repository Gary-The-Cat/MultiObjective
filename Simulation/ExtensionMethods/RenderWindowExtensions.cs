using Game.SFML_Text;
using SFML.Graphics;
using SFML.System;

namespace Game.ExtensionMethods
{
    public static class RenderWindowExtensions
    {
        public static void Draw(this RenderWindow window, Sprite texture, float scale, Vector2f position)
        {
            texture.Scale = new Vector2f(scale, scale);
            texture.Position = new Vector2f(position.X, position.Y);
            window.Draw(texture);
        }

        public static void DrawString(this RenderWindow window, FontText textFont, Vector2f position, bool centre = true)
        {
            var text = new Text(textFont.StringText, textFont.Font);
            var size = text.GetLocalBounds();
            var scale = textFont.Scale;
            var textWidth = size.Width * scale;
            var textHeight = size.Height * scale;
            text.Scale = new Vector2f(scale, scale);
            text.Position = centre
                ? new Vector2f(position.X - textWidth / 2, position.Y - textHeight / 2)
                : new Vector2f(position.X, position.Y);
            text.FillColor = textFont.TextColour;
            text.OutlineColor = textFont.TextColour;
            window.Draw(text);
        }
    }
}
