using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.Models;
public class Log(string title, string description, LogType type)
{
    public int Id { get; set; }
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
    public LogType Type { get; set; } = type;
    public DateTime Created { get; set; } = DateTime.UtcNow;
}

