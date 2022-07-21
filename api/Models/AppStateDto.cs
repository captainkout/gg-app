namespace api.Models;

public class AppStateDto
{
    public int QueueLength { get; set; }
    public int MemoryUsage { get; set; }
    public int IntFibCacheLength { get; set; }
}
