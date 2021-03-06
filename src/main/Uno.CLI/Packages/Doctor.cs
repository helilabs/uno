﻿using System.Collections.Generic;
using Mono.Options;
using Uno.Build;
using Uno.Build.Packages;
using Uno.Build.Targets;

namespace Uno.CLI.Packages
{
    class Doctor : Command
    {
        public override string Name => "doctor";
        public override string Description => "Repair/rebuild packages found in search paths.";

        public override void Help()
        {
            WriteUsage("[options] [package ...]");

            WriteHead("Available options", 27);
            WriteRow("-a, --all",                  "Build all projects, regardless of modification time");
            WriteRow("-f, --force",                "Update all package caches, regardless of modification time");
            WriteRow("-e, --express",              "Express mode. Don't rebuild packages depending on a modified package");
            WriteRow("-z, --clean",                "Clean projects before building them");
            WriteRow("-c, --configuration=NAME",   "Set build configuration (Debug|Release)", true);
            WriteRow("-b, --build-number=VERSION", "Override version for all packages built", true);
            WriteRow("-C, --no-cache",             "Disable in-memory AST & IL caches");
            WriteRow("-s, --silent",               "Very quiet build log");
        }

        public override void Execute(IEnumerable<string> args)
        {
            var force = false;
            var lib = new LibraryBuilder(Disk, BuildTargets.Package);
            lib.RebuildList = new OptionSet {
                    { "a|all", value => lib.RebuildAll = true },
                    { "f|force", value => force = true },
                    { "x|e|express", value => lib.Express = true },
                    { "z|clean", value => lib.Clean = true },
                    { "c=|configuration=", value => lib.Configuration = value.ParseEnum<BuildConfiguration>("configuration") },
                    { "b=|build-number=", value => lib.Version = value.ParseString("build-number") },
                    { "C|no-cache", value => lib.CanCache = false },
                    { "s|silent", value => lib.SilentBuild = true },
                }.Parse(args);

            Log.ProductHeader();

            // Repair package caches first
            new PackageDoctor(Log)
                .Repair(lib.RebuildList, force);

            lib.Build();
        }
    }
}
