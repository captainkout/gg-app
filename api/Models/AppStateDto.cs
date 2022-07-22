namespace Api.Models;

public class AppState
{
    public int QueueLength { get; set; }
    public long MemoryUsage { get; set; }
    public int CacheCnt { get; set; }
    public int CacheAge { get; set; }
}

public class AppStateDto
{
    public int QueueLength { get; set; }
    public long MemoryUsage { get; set; }
    public int CacheCnt { get; set; }
    public int CacheAge { get; set; }
}
