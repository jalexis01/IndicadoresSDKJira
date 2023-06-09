using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbLogExecutionProcessor
{
    public long Id { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime InitDate { get; set; }

    public DateTime EndDate { get; set; }

    public long? IdLogMessageInInit { get; set; }

    public long? IdLogMessageInEnd { get; set; }

    public string? Observations { get; set; }
}
