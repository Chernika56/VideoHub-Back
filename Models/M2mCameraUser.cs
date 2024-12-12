using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class M2mCameraUser
{
    public uint CuId { get; set; }

    public uint CuUsrId { get; set; }

    public uint CuCamId { get; set; }

    public virtual Camera CuCam { get; set; } = null!;

    public virtual User CuUsr { get; set; } = null!;
}
