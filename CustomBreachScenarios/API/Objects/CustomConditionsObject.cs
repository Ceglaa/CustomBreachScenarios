namespace CustomBreachScenarios.API.Objects
{
    /// <summary>
    /// Object for custom conditions.
    /// </summary>
    public class CustomConditionsObject
    {
        /// <summary>
        /// Gets or sets a value indicating whether if NTF can spawn or not.
        /// </summary>
        public bool CanNtfSpawn { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether if CHI can spawn or not.
        /// </summary>
        public bool CanChiSpawn { get; set; } = true;
    }
}
