﻿using System;
using System.Collections.Generic;

namespace Follow_Up_Manager.Models;

public partial class User
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long? Status { get; set; }

    public byte[]? CreatedAt { get; set; }

    public byte[]? UpdatedAt { get; set; }
}
