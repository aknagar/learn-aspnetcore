﻿using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace Route.Api
{
    internal class PrefixKeyVaultSecretManager : IKeyVaultSecretManager
    {
        private string versionPrefix;

        public PrefixKeyVaultSecretManager(string versionPrefix)
        {
            versionPrefix = $"{versionPrefix}-";
        }

        public bool Load(SecretItem secret)
        {
            // Load a vault secret when its secret name starts with the 
            // prefix. Other secrets won't be loaded.
            return secret.Identifier.Name.StartsWith(versionPrefix);
        }

        public string GetKey(SecretBundle secret)
        {
            // Remove the prefix from the secret name and replace two 
            // dashes in any name with the KeyDelimiter, which is the 
            // delimiter used in configuration (usually a colon). Azure
            // Key Vault doesn't allow a colon in secret names.
            return secret.SecretIdentifier.Name
                  .Substring(versionPrefix.Length)
                  .Replace("--", ConfigurationPath.KeyDelimiter);
        }
    }
}