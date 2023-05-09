using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbEndPoint
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Version { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public string? AzureLocation { get; set; }

    public virtual ICollection<TbLogRequestsIn> TbLogRequestsIns { get; set; } = new List<TbLogRequestsIn>();
}
