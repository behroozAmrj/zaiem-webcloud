using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Foundation.Infrastructure
{
    public interface INetworkService
    {
        Boolean upload(String URL, byte[] fileContent);
        StreamReader download(String URL);
        Stream displayContent(String URL);
        void excuteCommand(INetworkServiceCommand command);
    }
}
