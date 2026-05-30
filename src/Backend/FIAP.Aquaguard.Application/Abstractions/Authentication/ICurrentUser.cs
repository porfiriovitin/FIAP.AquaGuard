using FIAP.AquaGuard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIAP.Aquaguard.Application.Abstractions.Authentication;


public interface ICurrentUser
{
    Guid UserId { get; }
    UserRole Role { get; }
    bool IsAuthenticated { get; }
}
