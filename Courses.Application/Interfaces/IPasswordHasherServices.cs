using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Application.Interfaces;

public interface IPasswordHasherServices
{
    string Hash(string password);
    bool Verify(string Password, string PasswordHash);
}
