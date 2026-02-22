#region Using

using Blackfish.Shared;
using Blackfish.Studios;


#endregion

namespace Blackfish.Campaigns;

public partial class Campaign : MaterialBranchNode
{
    #region Properties

    public Studio? Studio { get; set; }
    public Guid? StudioId { get; set; }

    #endregion
}