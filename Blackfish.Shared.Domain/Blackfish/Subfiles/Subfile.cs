using System;
using System.Collections.Generic;
using System.Text;
using Blackfish.Materials;
using Blackfish.Shared;

namespace Blackfish.Subfiles;


public partial class Subfile: MaterialBranchNode
{
    public Guid? MaterialId { get; set; }
    public Material? Material { get; set; }
}

