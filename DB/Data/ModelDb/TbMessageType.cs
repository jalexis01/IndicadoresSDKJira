using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbMessageType
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string TableName { get; set; } = null!;

    public bool? Enable { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string FieldCode { get; set; } = null!;

    public string FieldWeft { get; set; } = null!;

    public string? FieldIdentifierMessage { get; set; }

    public virtual ICollection<TbMessageTypeField> TbMessageTypeFields { get; set; } = new List<TbMessageTypeField>();
}
