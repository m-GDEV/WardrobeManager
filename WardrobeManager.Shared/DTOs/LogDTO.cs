using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.DTOs;

public class LogDTO
{
   public required string Title { get; set; } 
   public required string Description { get; set; } 
   public LogType Type { get; set; }
   public LogOrigin Origin { get; set; }
   public DateTime Created { get; set; }
}