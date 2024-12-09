namespace TaskManager.Srv.Utilities.Exceptions;

/// <summary>
/// Saját kivételkezelő osztály
/// </summary>
public class NonFatalException : Exception
{
    public NonFatalException(string message) : base(message)
    {

    }

    public NonFatalException(string message, Exception inner) : base(message, inner)
    {

    }
}

