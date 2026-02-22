#region Using

using Blackfish.Campaigns;
using Blackfish.Shared;

#endregion

namespace Blackfish.Materials;

public partial class Material : MaterialBranchNode
{
    #region Properties

    public Campaign? Campaign { get; set; }
    public Guid? CampaignId { get; set; }

    #endregion
}