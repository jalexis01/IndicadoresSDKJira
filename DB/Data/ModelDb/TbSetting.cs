using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbSetting
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;

    public int? IdParent { get; set; }

    public string? Description { get; set; }

    public virtual TbSetting? IdParentNavigation { get; set; }

    public virtual ICollection<TbSetting> InverseIdParentNavigation { get; set; } = new List<TbSetting>();
}
