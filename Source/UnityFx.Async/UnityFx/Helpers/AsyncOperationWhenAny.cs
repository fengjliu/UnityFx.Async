﻿// Copyright (c) Alexander Bogarsukov.
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

namespace UnityFx.Async
{
	/// <summary>
	/// An operation that finishes when any of the child operations finish.
	/// </summary>
	internal sealed class AsyncOperationWhenAny : AsyncResult, IAsyncOperationContainer
	{
		#region data

		private List<IAsyncResult> _ops;
		private AsyncContinuationOptions _options;

		#endregion

		#region interface

		public AsyncOperationWhenAny(IAsyncResult[] ops, AsyncContinuationOptions options)
			: base(null)
		{
			Initialize(ops, options);
		}

#if !UNITYFX_NET35
		public AsyncOperationWhenAny(IAsyncResult[] ops, CancellationToken cancellationToken, AsyncContinuationOptions options)
			: base(null, cancellationToken)
		{
			Initialize(ops, options);
		}
#endif

		#endregion

		#region AsyncResult

		protected override void OnUpdate()
		{
			var allCompleted = true;

			foreach (var op in _ops)
			{
				if (op.IsCompleted)
				{
					if (IsCompletedWithOptions(op, _options))
					{
						SetCompleted();
						return;
					}
				}
				else
				{
					if (op is IAsyncOperation asyncOp)
					{
						if (asyncOp.Progress > Progress)
						{
							SetProgress(asyncOp.Progress);
						}
					}

					allCompleted = false;
				}
			}

			if (allCompleted)
			{
				SetException(null);
			}
		}

		#endregion

		#region IAsyncOperationContainer

		public int Size
		{
			get
			{
				return 1;
			}
		}

		#endregion

		#region implementation

		private void Initialize(IAsyncResult[] ops, AsyncContinuationOptions options)
		{
			_ops = new List<IAsyncResult>(ops.Length);
			_options = options;

			foreach (var op in ops)
			{
				if (op != null)
				{
					_ops.Add(op);
				}
			}
		}

		#endregion
	}
}