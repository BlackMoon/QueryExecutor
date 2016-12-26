using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using queryExecutor.CQRS.Command;

namespace queryExecutor.Domain.DscQueryData.Command
{
    public class DscQDataCommand : ICommand
    {
        public string DataSource { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }

        public string Code { get; set; }

        public string Path { get; set; }
    }
}