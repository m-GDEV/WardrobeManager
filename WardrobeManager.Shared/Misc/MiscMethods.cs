namespace WardrobeManager.Shared.Misc;

public static class MiscMethods
{
    // This is by no means actually random
    public static string GenerateRandomId()
    {
        Random random = new Random();
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var id = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        return $"id{id}"; // id needs to start with letter
    }
}