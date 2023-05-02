using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Interface for objects that can be saved.
/// </summary>
public interface ISavable
{
    /// <summary>
    /// Captures the current state of the object.
    /// </summary>
    object CaptureState();

    /// <summary>
    /// Restores the state of the object.
    /// </summary>
    /// <param name="state">The state to be restored.</param>
    void RestoreState(object state);
}
