using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class FibRequestDto
{
    [Required]
    [Range(0, int.MaxValue)]
    public int StartIndex { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int EndIndex { get; set; }
    public bool Cache { get; set; }

    /// <summary>
    /// Time in milliseconds
    /// </summary>
    /// <value></value>
    [Required]
    [Range(1, int.MaxValue)]
    public int MaxTime { get; set; }

    /// <summary>
    /// Max in kb
    /// </summary>
    /// <value></value>
    public int MaxMemory { get; set; }
}
