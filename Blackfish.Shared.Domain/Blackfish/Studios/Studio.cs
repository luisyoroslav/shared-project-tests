using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Blackfish.Campaigns;
using Blackfish.Shared;

namespace Blackfish.Studios;

public partial class Studio: MaterialBranchNode
{
    public ICollection<Campaign> Campaigns { get; set; } = new ReadOnlyCollection<Campaign>(new List<Campaign>());
}