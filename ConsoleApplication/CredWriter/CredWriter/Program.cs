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
                CredentialType.Generic,
                CredentialManager.CredentialPersistence.Session);
        }

        private static void SetCredentialAccessToken()
        {
            CredentialManager.Write(
                "git:https://source.developers.google.com",
                CredentialType.Generic,
                CredentialManager.CredentialPersistence.Session);
        }

        static void Main(string[] args)
        {
            //SetCredential();
            SetCredentialAccessToken();
        }
    }
}
