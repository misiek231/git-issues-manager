using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitIssuesManager.Logic.Models;

public class CreateIssueModel
{
    public string? Title { get; set; }
    public string? Body { get; set; }
}

public class UpdateIssueModel
{
    public string? Title { get; set; }
    public string? Body { get; set; }
}