using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbLogHistoryMessageIn
{
    public long Id { get; set; }

    public string Message { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public string? Observations { get; set; }

    public bool Processed { get; set; }

    public int IdProcessed { get; set; }

    public DateTime? DateProcessed { get; set; }

    public long? IdHeaderMessage { get; set; }

    public virtual TbHeaderMessage? IdHeaderMessageNavigation { get; set; }
}
