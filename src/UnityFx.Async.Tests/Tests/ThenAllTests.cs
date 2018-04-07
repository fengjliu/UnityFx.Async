﻿// Copyright (c) Alexander Bogarsukov.
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnityFx.Async
{
	public class ThenAllTests
	{
		[Fact]
		public async Task ThenAll_CompletesWhenAllOperationsComplete()
		{
			// Arrange
			var op1 = AsyncResult.Delay(1);
			var op2 = AsyncResult.Delay(2);
			var op3 = AsyncResult.Delay(3);

			// Act
			await op1.ThenAll(() => new IAsyncOperation[] { op2, op3 });

			// Assert
			Assert.True(op1.IsCompleted);
			Assert.True(op2.IsCompleted);
			Assert.True(op3.IsCompleted);
		}
	}
}
