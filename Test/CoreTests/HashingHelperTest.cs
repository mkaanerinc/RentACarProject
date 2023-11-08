using Core.Utilities.Security.Hashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.CoreTests
{
    public class HashingHelperTest
    {
        private string _password = "mysupersecretkey33";

        [Fact]
        public void CreatePassword_ActionExecutes_ReturnByteArray()
        {

            HashingHelper.CreatePasswordHash(_password, out byte[] passwordHash, out byte[] passwordSalt);

            Assert.NotNull(passwordHash);
            Assert.NotNull(passwordSalt);
            Assert.IsType<byte[]>(passwordHash);
            Assert.IsType<byte[]>(passwordSalt);
        }

        [Fact]
        public void VerifyPassword_ActionExecutes_ReturnTrue()
        {
            HashingHelper.CreatePasswordHash(_password, out byte[] passwordHash, out byte[] passwordSalt);


            var result = HashingHelper.VerifyPasswordHash(_password, passwordHash, passwordSalt);

            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_PasswordIsInvalid_ReturnFalse()
        {
            string password = "mysupersecretkey06";

            HashingHelper.CreatePasswordHash(_password, out byte[] passwordHash, out byte[] passwordSalt);

            var result = HashingHelper.VerifyPasswordHash(password, passwordHash, passwordSalt);

            Assert.False(result);
        }
    }
}
