namespace api.Models;

public class FibRequestDto
{
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
    public bool Cache { get; set; }

    /// <summary>
    /// Time in milliseconds
    /// </summary>
    /// <value></value>
    public int MaxTime { get; set; }

    /// <summary>
    /// Max in bytes
    /// </summary>
    /// <value></value>
    public int MaxMemory { get; set; }
}
