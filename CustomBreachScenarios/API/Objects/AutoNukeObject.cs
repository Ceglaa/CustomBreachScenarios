namespace CustomBreachScenarios.API.Objects
{
    using System.ComponentModel;

    using GameCore;

    /// <summary>
    /// Auto Nuke Object.
    /// </summary>
    public class AutoNukeObject
    {
        /// <summary>
        /// Gets or sets Auto Nuke delay.
        /// </summary>
        [Description("Warhead starts automaticly after delay. If set to 0 it will not start")]
        public int Delay { get; set; }

        /// <summary>
        /// Gets or sets Auto Nuke timer time.
        /// </summary>
        public int Time { get; set; } = ConfigFile.ServerConfig.GetInt("warhead_tminus_start_duration", 90);

        /// <summary>
        /// Gets or sets Auto Nuke chance.
        /// </summary>
        public int Chance { get; set; }
    }
}
