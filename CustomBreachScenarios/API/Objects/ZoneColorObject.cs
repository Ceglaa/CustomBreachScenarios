namespace CustomBreachScenarios.API.Objects
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Serializable color class.
    /// </summary>
    [Serializable]
    public class ZoneColorObject
    {
        /// <summary>
        /// Gets or sets Red value.
        /// </summary>
        public float R { get; set; }

        /// <summary>
        /// Gets or sets Green value.
        /// </summary>
        public float G { get; set; }

        /// <summary>
        /// Gets or sets Blue value.
        /// </summary>
        public float B { get; set; }

        /// <summary>
        /// Gets or sets Alpha value.
        /// </summary>
        public float A { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneColorObject"/> class.
        /// </summary>
        public ZoneColorObject()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneColorObject"/> class.
        /// </summary>
        /// <param name="r">Red.</param>
        /// <param name="g">Green.</param>
        /// <param name="b">Blue.</param>
        /// <param name="a">Alpha.</param>
        public ZoneColorObject(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}