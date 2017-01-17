using System;
using System.IdentityModel.Selectors;

namespace queryExecutor.Service
{
    public class CredentialValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (null == userName)
                throw new ArgumentNullException(nameof(userName));

            if (null == password)
                throw new ArgumentNullException(nameof(password));
        }
    }
}