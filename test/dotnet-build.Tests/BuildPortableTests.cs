﻿using System.IO;
using Microsoft.DotNet.Tools.Test.Utilities;
using Xunit;

namespace Microsoft.DotNet.Tools.Builder.Tests
{
    public class BuildPortableTests : TestBase
    {
        [Fact]
        public void BuildingAPortableProjectProducesDepsFile()
        {
            var testInstance = TestAssetsManager.CreateTestInstance("PortableTests")
                .WithLockFiles();

            var result = new BuildCommand(
                projectPath: Path.Combine(testInstance.TestRoot, "PortableApp"),
                forcePortable: true)
                .ExecuteWithCapturedOutput();

            result.Should().Pass();

            var outputBase = new DirectoryInfo(Path.Combine(testInstance.TestRoot, "bin", "Debug"));

            var netstandardappOutput = outputBase.Sub("netstandard1.5");

            netstandardappOutput.Should()
                .Exist().And
                .HaveFiles(new[]
                {
                    "BuildTestPortableProject.deps",
                    "BuildTestPortableProject.deps.json",
                    "BuildTestPortableProject.dll",
                    "BuildTestPortableProject.pdb"
                });
        }
    }
}
