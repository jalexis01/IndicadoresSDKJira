using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbValidField
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? DataType { get; set; }

    public bool SearchType { get; set; }

    public bool? PrimaryData { get; set; }

    public virtual ICollection<TbMessageTypeField> TbMessageTypeFields { get; set; } = new List<TbMessageTypeField>();
}
