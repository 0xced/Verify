﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Verify;

namespace VerifyNUnit
{
    public static partial class Verifier
    {
        public static async Task Verify<T>(
            Task<T> target,
            VerifySettings? settings = null,
            [CallerFilePath] string sourceFile = "")
        {
            using var verifier = BuildVerifier(sourceFile);
            await verifier.Verify(target, settings);
        }

        public static Task Verify<T>(
            IAsyncEnumerable<T> target,
            VerifySettings? settings = null,
            [CallerFilePath] string sourceFile = "")
        {
            var verifier = BuildVerifier(sourceFile);
            return verifier.Verify(target, settings);
        }

        public static async Task Verify<T>(
            T target,
            VerifySettings? settings = null,
            [CallerFilePath] string sourceFile = "")
        {
            using var verifier = BuildVerifier(sourceFile);
            await verifier.Verify(target, settings);
        }
    }
}