using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbParameter
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string? Description { get; set; }
}
