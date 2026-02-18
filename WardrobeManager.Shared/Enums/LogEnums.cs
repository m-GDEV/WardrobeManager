namespace WardrobeManager.Shared.Enums;

public enum LogType
{
    Info,
    Warning,
    Error,
    UncaughtException
}

public enum LogOrigin
{
    Frontend,
    Backend,
    Database,
    Unknown
}
