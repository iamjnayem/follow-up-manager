using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Follow_Up_Manager.interfaces;

public interface INotificationService
{
    Task<bool> Subscribe(string endpoint, string p256dh, string auth);
}