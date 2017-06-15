using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CredWriter
{


    class Program
    {
        private static void SetCredential()
        {
            CredentialManager.Write(
                "git:https://source.developers.google.com",
                username: "VisualStudioUser",
                credentialType: CredentialType.Generic,
                persistenceType: CredentialManager.CredentialPersistence.LocalMachine);
        }

        private static void SetUrlCredential()
        {
            CredentialManager.Write(
                "git:https://source.developers.google.com/p/pacific-wind/r/hang",
                username: "VisualStudioUser",
                credentialType: CredentialType.Generic,
                persistenceType: CredentialManager.CredentialPersistence.LocalMachine);
        }

        private static void SetCredentialAccessToken()
        {
            CredentialManager.Write(
                "git:https://source.developers.google.com",
                username: "VisualStudioUser",
                password: "wrong",
                credentialType: CredentialType.Generic,
                persistenceType: CredentialManager.CredentialPersistence.LocalMachine);
        }

        static void Main(string[] args)
        {
            //SetCredential();
            //SetCredentialAccessToken();
            //SetUrlCredential();
            SetCredential();
        }
    }
}
