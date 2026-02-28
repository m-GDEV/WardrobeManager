namespace WardrobeManager.Api.Services.Interfaces;

public interface IFileService
{
    /// <summary>Gets a specific image based on the guid</summary>
    /// <param name="guid">GUID of image</param>
    /// <returns>byte array of image </returns>
    Task<byte[]> GetImage(string guid);
    string ParseGuid(Guid guid);
    Task SaveImage(Guid? guid, string imageBase64);
    Task DeleteImage(Guid givenGuid);
}
