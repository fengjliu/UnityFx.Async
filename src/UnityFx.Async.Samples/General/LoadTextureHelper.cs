﻿// Copyright (c) Alexander Bogarsukov.
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityFx.Async.Samples
{
	/// <summary>
	/// A <see cref="MonoBehaviour"/> that demonstrates wrapping <c>Unity3d</c> web requests in Task-based Asynchronous Pattern manner.
	/// </summary>
	public partial class LoadTextureHelper : MonoBehaviour
	{
		/// <summary>
		/// Asynchronously loads a <see cref="Texture2D"/> from the specified URL.
		/// </summary>
		public IAsyncOperation<Texture2D> LoadTextureAsync(string textureUrl)
		{
			var result = new AsyncCompletionSource<Texture2D>();
			StartCoroutine(LoadTextureInternal(result, textureUrl));
			return result.Operation;
		}

		private IEnumerator LoadTextureInternal(IAsyncCompletionSource<Texture2D> op, string textureUrl)
		{
			var www = UnityWebRequestTexture.GetTexture(textureUrl);
			yield return www.Send();

			if (www.isNetworkError || www.isHttpError)
			{
				op.SetException(new Exception(www.error));
			}
			else
			{
				op.SetResult(((DownloadHandlerTexture)www.downloadHandler).texture);
			}
		}
	}
}
