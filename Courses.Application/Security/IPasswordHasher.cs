using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Application.Security;

public interface IPasswordHasher
{
    Task<bool> VerifyAsync(string password, string encodedHash);
}
