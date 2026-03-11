#region Using

using System.Collections.ObjectModel;
using Blackfish.Campaigns;
using Blackfish.Shared;

#endregion

namespace Blackfish.Studios;

public class Studio : MaterialBranchNode
{
    #region Properties

    public ICollection<Campaign> Campaigns { get; set; } = new ReadOnlyCollection<Campaign>(new List<Campaign>());

    #endregion
}