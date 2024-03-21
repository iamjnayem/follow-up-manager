using System;
using System.Collections.Generic;

namespace Follow_Up_Manager.Models;

public partial class Notification
{
    public long Id { get; set; }

    public long? Status { get; set; }

    public string? Endpoint { get; set; }

    public string? P256dh { get; set; }

    public string? Auth { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}
