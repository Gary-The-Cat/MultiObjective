using Game.GeneticAlgorithm;
using Game.Helpers;
using Game.Screens;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Game
{
    public class Game
    {
        // Main Window
        private readonly RenderWindow window;

        // Screen Manager
        private readonly ScreenManager screenManager;

        // Screens
        private readonly SimulationScreen pathScreen;
        private readonly ParetoVisualScreen paretoScreen;

        // Timings & Clocks
        private readonly Clock clock;
        private readonly Clock generationClock;
        private readonly float generationTime = 0.2f;

        // GA Specific
        private readonly World world;
        private Task doGeneration;

        public Game()
        {
            // Create the main window
            window = new RenderWindow(
                new VideoMode(Configuration.Width, Configuration.Height), 
                "World Simulation", 
                Styles.Fullscreen,
                new ContextSettings() { AntialiasingLevel = 8 });

            // Set our frame rate to 60fps so the screen is responsive.
            window.SetFramerateLimit(60);

            // Handle window events
            window.Closed += OnClose;
            window.Resized += OnResize;

            // Create our world
            world = new World();

            screenManager = new ScreenManager(window);

            // Create a simulation screen. Note that screens can be stacked on top of one another
            // in the screen manager that has been omitted from this specific repository.
            pathScreen = new SimulationScreen(window, Configuration.SinglePlayer);
            paretoScreen = new ParetoVisualScreen(window, Configuration.SinglePlayer, world.Population);

            // Add the screens to the manager
            screenManager.AddScreen(paretoScreen);
            screenManager.AddScreen(pathScreen);

            // Configure the pareto screen to be centred, hidden & appropriately sized
            SetParetoConfiguration();

            // Clock to track frame time. If we dont do this, and instead assume we are hitting 60fps, we can get stutters 
            // in our camera movement.
            clock = new Clock();

            // Create a new generation clock to only DoGeneration once per second.
            generationClock = new Clock();
        }

        public void Run()
        {
            // Spawn our initial populaiton of individuals
            world.Spawn();

            // Set our generation count to zero and restart our timer
            int generation = 0;
            generationClock.Restart();

            while (window.IsOpen)
            {
                // Get the amount of time that has passed since we drew the last frame.
                float deltaT = clock.Restart().AsMicroseconds() / 1000000f;

                // Clear the previous frame
                window.Clear(Configuration.Background);
                
                // Process events
                window.DispatchEvents();

                // Camera movement is now separate from our other screen update code to minimise overhead.
                screenManager.UpdateCamera(deltaT);

                screenManager.Draw(deltaT);

                // Update the window
                window.Display();

                // If one second has passed, breed the next generation
                if(generationClock.ElapsedTime.AsSeconds() > generationTime &&
                    (doGeneration?.IsCompleted ?? true) &&
                    !world.HasConverged)
                {
                    doGeneration = Task.Run(() =>
                    {
                        // Do one generation of breeding & culling.
                        world.DoGeneration();

                        // Restart our generation timer
                        generationClock.Restart();

                        // Update the screen to show the new generation
                        pathScreen.GenerationString.StringText = $"Generation: {++generation}";

                        // Draw the paths of the current best individual
                        pathScreen.UpdateSequence(world.GetBestIndividual());

                        // Update all the screen components that are unrelated to our sequence.
                        screenManager.Update(deltaT);
                    });
                }

                // Our GA has either hit the max generations or has stopped improving, set the GA as complete
                // and copy the history of fitness values to the clipboard
                if(world.GenerationCount == GAConfig.MaxGenerations || 
                    world.NoImprovementCount == GAConfig.MaxNoImprovementCount)
                {
                    pathScreen.SetGACompleted();

                    // Copy the fitness to clipboard
                    Clipboard.SetText(string.Join(",", world.FitnessOverTime));
                }

                // Process any key presses that the user has made.
                this.ProcessUserInput();

                // Check to see if the user wants to stop the simulation.
                if (Keyboard.IsKeyPressed(Configuration.QuitKey) || 
                    Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    return;
                }
            }
        }

        private void ProcessUserInput()
        {
            // If the spacebar has been just pressed, toggle the pareto visualisation
            if (KeyboardHelper.IsKeyJustPressed(Configuration.ParetoVisualisationKey))
            {
                paretoScreen.SetActiveState(!paretoScreen.IsDraw);
            }
        }

        private void SetParetoConfiguration()
        {
            // Set the position & Size
            if (Configuration.UseRandomTowns)
            {
                paretoScreen.Camera.Position = 
                    new Vector2f(
                        (Configuration.Width / Configuration.Scale) / 2, 
                        (Configuration.Height / Configuration.Scale) / 2);

                paretoScreen.Camera.GetView().Zoom(0.3f);
            }

            // Set the screen to not be shown on load
            paretoScreen.IsDraw = false;
        }

        private void OnResize(object sender, SizeEventArgs e)
        {
            var window = (RenderWindow)sender;
            pathScreen.Camera.ScaleToWindow(window.Size.X, window.Size.Y);
        }

        private static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            ((RenderWindow)sender).Close();
        }
    }
}