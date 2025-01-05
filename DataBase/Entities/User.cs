using System;
using System.Collections.Generic;

namespace BackEnd.DataBase.Entities;

public partial class User
{
    public uint UsrId { get; set; }

    public string? UsrName { get; set; }

    public string UsrLogin { get; set; } = null!;

    public string UsrPassword { get; set; } = null!;

    public string? UsrEmail { get; set; }

    public string UsrRole { get; set; }

    public virtual ICollection<M2mCameraUser> M2mCameraUsers { get; set; } = new List<M2mCameraUser>();
}
