using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbMessageTypeField
{
    public int Id { get; set; }

    public int IdMessageType { get; set; }

    public int IdValidField { get; set; }

    public string? CustomName { get; set; }

    public bool? Enable { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual TbMessageType IdMessageTypeNavigation { get; set; } = null!;

    public virtual TbValidField IdValidFieldNavigation { get; set; } = null!;
}
