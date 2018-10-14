using System;
using System.Collections.Generic;
using System.Text;

namespace TutorialCode
{
    interface ISample
    {
        /// <summary>
        /// Runs the sample.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <param name="tutRoot">Root directory of the tutorials</param>
        void Run(string[] args, string tutRoot);
    }
}
