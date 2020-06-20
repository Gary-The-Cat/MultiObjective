using System.Collections.Generic;
using static SFML.Window.Keyboard;

namespace Game.Helpers
{
    /// <summary>
    /// Keyboard helper aids in simplifying some slightly more advanced interactions. 
    /// Currently only has support for 'KeyJustPressed' indicating it went from not pressed
    /// to pressed, this frame.
    /// </summary>
    public static class KeyboardHelper
    {
        private static HashSet<Key> pressedKeys = new HashSet<Key>();

        /// <summary>
        /// IsKeyJustPressed should be called every frame in order to update its state.
        /// </summary>
        /// <param name="key">The desired key.</param>
        /// <returns>true or false indicating if the provided key was pressed this frame.</returns>
        public static bool IsKeyJustPressed(Key key)
        {
            bool isKeyPressed = IsKeyPressed(key);

            if (!isKeyPressed)
            {
                pressedKeys.Remove(key);
                return false;
            }
            else
            {
                if (pressedKeys.Contains(key))
                {
                    return false;
                }

                pressedKeys.Add(key);
                return true;
            }
        }

    }
}
