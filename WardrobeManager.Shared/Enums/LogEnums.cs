namespace WardrobeManager.Shared.Enums;

public enum LogType
{
    Info,
    Warning,
    Error,
    UncaughtException,
    RequestLog // logging incoming requests
}

public enum LogOrigin
{
    Frontend,
    Backend,
    Database,
    Unknown
}
