#region Using

using Blackfish.Materials;
using Blackfish.Shared;

#endregion

namespace Blackfish.Subfiles;

public class Subfile : MaterialBranchNode
{
    #region Properties

    public Material? Material { get; set; }
    public Guid? MaterialId { get; set; }

    #endregion
}