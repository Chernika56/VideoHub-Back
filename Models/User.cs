using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class User
{
    public uint UsrId { get; set; }

    public string? UsrName { get; set; }

    public string? UsrLogin { get; set; }

    public string? UsrPassword { get; set; }

    public string? UsrEmail { get; set; }

    public virtual ICollection<M2mCameraUser> M2mCameraUsers { get; set; } = new List<M2mCameraUser>();
}
