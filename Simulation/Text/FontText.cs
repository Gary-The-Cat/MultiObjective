using SFML.Graphics;

namespace Game.SFML_Text
{
    public class FontText
    {
        public FontText(Font font, string stringText, Color textColour, float scale = 1)
        {
            this.Font = font;
            this.StringText = stringText;
            this.TextColour = textColour;
            this.Scale = scale;
        }

        public Font Font;

        public string StringText;

        public Color TextColour;

        public float Scale;
    }
}
