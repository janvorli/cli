// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace Microsoft.DotNet.Tools.Test.Utilities
{
    public class DisableForRIDFactAttribute : FactAttribute
    {
        public DisableForRIDFactAttribute(string rid)
        {
            if (RepoDirectoriesProvider.BuildRid == rid)
            {
                this.Skip = $"This test is disabled for the current RID {rid}";
            }
        }
    }
}
