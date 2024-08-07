using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR.Cqrs.EntityFrameworkCore.Tests;
public class PingBook
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

}
