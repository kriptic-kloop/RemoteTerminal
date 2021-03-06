﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Renci.SshNet.Common;
using Renci.SshNet.Security.Cryptography.Ciphers;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;

namespace Renci.SshNet.Security.Cryptography
{
    /// <summary>
    /// Implements RSA digital signature algorithm.
    /// </summary>
    public class RsaDigitalSignature : CipherDigitalSignature, IDisposable
    {
        private CryptographicHash _hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaDigitalSignature"/> class.
        /// </summary>
        /// <param name="rsaKey">The RSA key.</param>
        public RsaDigitalSignature(RsaKey rsaKey)
            : base(new ObjectIdentifier(1, 3, 14, 3, 2, 26), new RsaCipher(rsaKey))
        {
            this._hash = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1).CreateHash();
        }

        /// <summary>
        /// Hashes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        protected override byte[] Hash(byte[] input)
        {
            this._hash.Append(CryptographicBuffer.CreateFromByteArray(input));
            byte[] hash;
            CryptographicBuffer.CopyToByteArray(this._hash.GetValueAndReset(), out hash);
            return hash;
        }

        #region IDisposable Members

        private bool _isDisposed = false;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged ResourceMessages.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged ResourceMessages.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._isDisposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged ResourceMessages.
                if (disposing)
                {
                    // Dispose managed ResourceMessages.
                    if (this._hash != null)
                    {
                        this._hash = null;
                    }
                }

                // Note disposing has been done.
                this._isDisposed = true;
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SshCommand"/> is reclaimed by garbage collection.
        /// </summary>
        ~RsaDigitalSignature()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion
    }
}
