using System;
using System.Collections.Generic;
using System.Text;

namespace JS.Sample.Application.Interfaces.Header
{
    public interface IHeaderService
    {
        Guid GetRequestId();
    }
}
