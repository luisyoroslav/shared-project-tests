#region Using

using System.Collections.ObjectModel;
using Blackfish.Subfiles;

#endregion

namespace Blackfish.Materials;

public partial class Material
{
    #region Properties

    public ICollection<Subfile> Subfiles { get; set; } = new ReadOnlyCollection<Subfile>(new List<Subfile>());

    #endregion
}