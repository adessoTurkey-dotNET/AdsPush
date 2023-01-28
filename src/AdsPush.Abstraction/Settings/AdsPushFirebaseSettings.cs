namespace AdsPush.Abstraction.Settings
{
    /// <summary>
    /// To be able to get this file you've to create a service account json file.
    /// See from: https://developers.google.com/identity/protocols/oauth2/service-account#creatinganaccount
    /// List service accounts: https://console.cloud.google.com/projectselector2/iam-admin/serviceaccounts?supportedpurview=project
    /// Service account should have "CloudMessagingAdmin" permission in rule while creating the service account.
    /// </summary>
    public class AdsPushFirebaseSettings
    {
        /// <summary>
        /// type filed in service_account.json
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// project_id filed in service_account.json
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// private_key_id filed in service_account.json
        /// </summary>
        public string PrivateKeyId { get; set; }

        /// <summary>
        /// private_key filed in service_account.json
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// client_email filed in service_account.json
        /// </summary>
        public string ClientEmail { get; set; }

        /// <summary>
        /// client_id filed in service_account.json
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// auth_uri filed in service_account.json
        /// </summary>
        public string AuthUri { get; set; }

        /// <summary>
        /// token_uri filed in service_account.json
        /// </summary>
        public string TokenUri { get; set; }

        /// <summary>
        /// auth_provider_x509_cert_url filed in service_account.json
        /// </summary>
        public string AuthProviderX509CertUrl { get; set; }

        /// <summary>
        /// client_x509_cert_url filed in service_account.json
        /// </summary>
        public string ClientX509CertUrl { get; set; }
    }
}
