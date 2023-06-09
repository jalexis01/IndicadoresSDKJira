using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbHeaderField
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string DataType { get; set; } = null!;

    public string? Description { get; set; }

    public string? CustomName { get; set; }

    public bool? Enabled { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool SearchType { get; set; }

    public bool PrimaryData { get; set; }
}
