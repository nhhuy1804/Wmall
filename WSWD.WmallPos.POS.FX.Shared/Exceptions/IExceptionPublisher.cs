using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.FX.Shared.Exceptions
{
    public interface IExceptionPublisher
    {
        string GetExceptionLogFolder();
        string GetExceptionLogFileName(bool unhandled);
        void ShowException(Exception ex);
        void ShowException(string exceptionText);
    }
}
