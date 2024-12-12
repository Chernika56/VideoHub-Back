using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Camera
{
    public uint CamId { get; set; }

    public string? CamUrl { get; set; }

    public string? CamType { get; set; }

    public int? CamArchiveTime { get; set; }

    public string? CamIp { get; set; }

    public string? CamName { get; set; }

    public virtual ICollection<M2mCameraUser> M2mCameraUsers { get; set; } = new List<M2mCameraUser>();
}
