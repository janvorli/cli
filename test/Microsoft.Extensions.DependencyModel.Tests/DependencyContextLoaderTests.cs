using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;
using FluentAssertions;
using Xunit;

namespace Microsoft.Extensions.DependencyModel.Tests
{
    public class DependencyContextTests
    {
        [Fact]
        public void MergeMergesLibraries()
        {
            var compilationLibraries = new[]
            {
                CreateCompilation("PackageA"),
                CreateCompilation("PackageB"),
            };

            var runtimeLibraries = new[]
            {
                CreateRuntime("PackageA"),
                CreateRuntime("PackageB"),
            };

            var compilationLibrariesRedist = new[]
            {
                CreateCompilation("PackageB"),
                CreateCompilation("PackageC"),
            };

            var runtimeLibrariesRedist = new[]
            {
                CreateRuntime("PackageB"),
                CreateRuntime("PackageC"),
            };

            var context = new DependencyContext(
                "Framework",
                "runtime",
                true,
                CompilationOptions.Default,
                compilationLibraries,
                runtimeLibraries,
                new KeyValuePair<string, string[]>[] { });

            var contextRedist = new DependencyContext(
                "Framework",
                "runtime",
                true,
                CompilationOptions.Default,
                compilationLibrariesRedist,
                runtimeLibrariesRedist,
                new KeyValuePair<string, string[]>[] { });

            var result = context.Merge(contextRedist);

            result.CompileLibraries.Should().BeEquivalentTo(new[]
            {
                compilationLibraries[0],
                compilationLibraries[1],
                compilationLibrariesRedist[1],
            });

            result.RuntimeLibraries.Should().BeEquivalentTo(new[]
            {
                runtimeLibraries[0],
                runtimeLibraries[1],
                runtimeLibrariesRedist[1],
            });
        }

        private CompilationLibrary CreateCompilation(string name)
        {
            return new CompilationLibrary(
                "project",
                name,
                "1.1.1",
                "HASH",
                new string[] { },
                new Dependency[] { },
                false);
        }

        private RuntimeLibrary CreateRuntime(string name)
        {
            return new RuntimeLibrary(
                "project",
                name,
                "1.1.1",
                "HASH",
                new RuntimeAssembly[] { },
                new ResourceAssembly[] { },
                new RuntimeTarget[] { },
                new Dependency[] {},
                false);
        }
    }
}
