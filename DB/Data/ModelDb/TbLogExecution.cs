using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbLogExecution
{
    public long Id { get; set; }

    public DateTime InitDateTime { get; set; }

    public string? Observations { get; set; }

    public DateTime? EndDateTime { get; set; }
}
