using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageFile.Models
{
  public class ActionRequest
  {
    public string Type { get; set; }
    public Dictionary<string, object> Params { get; set; }
  }
}