using System;

namespace LiveClientDesktop.Services
{
    public interface ILoginService
    {
        Tuple<bool, string> Login();
    }
}
