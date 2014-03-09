using System;
using System.Collections.Generic;
using System.Linq;

namespace PoroCYon.ICM
{
    // yep

    /// <summary>
    /// An exception used when something should not happen, except if someone is doing hacky stuff
    /// </summary>
    public class YoureAHackerException : Exception
    {
        /// <summary>
        /// Creates a new instance of the YoureAHackerException
        /// </summary>
        public YoureAHackerException()
            : this("You are a hacker!", null)
        {

        }
        /// <summary>
        /// Creates a new instance of the YoureAHackerException
        /// </summary>
        /// <param name="message">The message of the Exception</param>
        public YoureAHackerException(string message)
            : this(message, null)
        {

        }
        /// <summary>
        /// Creates a new instance of the YoureAHackerException
        /// </summary>
        /// <param name="inner">The Exception which was the cause of this Exception</param>
        public YoureAHackerException(Exception inner)
            : this("You are a hacker!", inner)
        {

        }
        /// <summary>
        /// Creates a new instance of the YoureAHackerException
        /// </summary>
        /// <param name="message">The message of the Exception</param>
        /// <param name="inner">The Exception which was the cause of this Exception</param>
        public YoureAHackerException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
