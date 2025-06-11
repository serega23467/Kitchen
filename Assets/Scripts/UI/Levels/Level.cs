using UnityEngine;

public class Level
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FolderName { get; set; }
    public string Description { get; set; }
    public string ImageName { get; set; }
    public int Rate { get; set; }
    public int Seconds { get; set; }
    public bool IsLocked { get; set; } = false;
}
