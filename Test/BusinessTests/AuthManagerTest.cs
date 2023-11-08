using Business.Abstract;
using Business.Concrete;
using Core.Entities.Concrete;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.BusinessTests
{
    public class AuthManagerTest
    {
        private readonly Mock<IUserService> _moqUserService;
        private readonly Mock<ITokenHelper> _moqTokenService;
        private readonly AuthManager _authManager;
        private List<OperationClaim> _claims;
        private AccessToken _accessToken;
        private User _user;

        public AuthManagerTest()
        {
            _moqUserService = new Mock<IUserService>();
            _moqTokenService = new Mock<ITokenHelper>();
            _authManager = new AuthManager(_moqUserService.Object, _moqTokenService.Object);

            _claims = new List<OperationClaim>() {
                new OperationClaim() { OperationClaimId = 1, Name = "admin" },
                new OperationClaim() { OperationClaimId = 2, Name = "moderator" },
                new OperationClaim() { OperationClaimId = 3, Name = "user" },
                new OperationClaim() { OperationClaimId = 4, Name = "car.add" },
                new OperationClaim() { OperationClaimId = 5, Name = "rental.list" },
            };
            _user = new User()
            {
                UserId = 1,
                Email = "usertest@test.com",
                FirstName = "User Test Name",
                LastName = "User Test LastName",
                Status = true,
                PasswordHash = CreateSpecialByteArray(1),
                PasswordSalt = CreateSpecialByteArray(1)
            };
            _accessToken = new AccessToken() { Expiration = new DateTime(2022, 8, 10, 10, 15, 0), Token = "testtoken" };
        }

        [Theory]
        [MemberData(nameof(AuthTestDataForValidCreateAccessToken))]
        public void CreateAccessToken_ActionExecutes_ReturnSuccessDataResultWithAccessToken(User user)
        {
            _moqUserService.Setup(x => x.GetClaims(user)).Returns(new SuccessDataResult<List<OperationClaim>>(_claims, "Claim'ler listelendi"));

            _moqTokenService.Setup(x => x.CreateToken(user, _claims)).Returns(_accessToken);

            var result = _authManager.CreateAccessToken(user);

            var returnDataResult = Assert.IsType<SuccessDataResult<AccessToken>>(result);

            _moqTokenService.Verify(x => x.CreateToken(user, _claims), Times.Once);
            _moqUserService.Verify(x => x.GetClaims(user), Times.Once);

            Assert.True(returnDataResult.Success);
            Assert.NotNull(returnDataResult.Data);
            Assert.Equal("Access Token oluşturuldu", returnDataResult.Message);
        }

        [Theory]
        [InlineData("usertest1@test.com")]
        public void UserExists_ActionExecutes_ReturnSuccessResult(string email)
        {
            User userNull = null;

            _moqUserService.Setup(x => x.GetByEmail(email)).Returns(new SuccessDataResult<User>(userNull, "Kullanıcılar listelendi"));

            var result = _authManager.UserExists(email);

            var returnResult = Assert.IsType<SuccessResult>(result);

            _moqUserService.Verify(x => x.GetByEmail(email), Times.Once);
            Assert.True(returnResult.Success);
            Assert.Null(returnResult.Message);
        }

        [Theory]
        [InlineData("usertest@test.com")]
        public void UserExists_UserIsExists_ReturnErrorResultWithMessage(string email)
        {
            _moqUserService.Setup(x => x.GetByEmail(email)).Returns(new SuccessDataResult<User>(_user, "Kullanıcılar listelendi"));

            var result = _authManager.UserExists(email);

            var returnResult = Assert.IsType<ErrorResult>(result);

            _moqUserService.Verify(x => x.GetByEmail(email), Times.Once);
            Assert.False(returnResult.Success);
            Assert.Equal("Kullanıcı kayıtlıdır", returnResult.Message);
        }

        [Theory]
        [MemberData(nameof(AuthTestDataForValidRegister))]
        public void Register_ActionExecutes_ReturnSuccessDataResultWithUser(UserForRegisterDto userForRegister, string password)
        {
            var result = _authManager.Register(userForRegister, password);

            var returnDataResult = Assert.IsType<SuccessDataResult<User>>(result);

            _moqUserService.Setup(x => x.Add(returnDataResult.Data)).Returns(new SuccessResult("Kullanıcı eklendi"));

            _moqUserService.Verify(x => x.Add(returnDataResult.Data), Times.Once);
            Assert.NotNull(returnDataResult.Data);
            Assert.True(returnDataResult.Success);
            Assert.Equal("Kullanıcı kayıt edildi", returnDataResult.Message);
        }

        [Theory]
        [MemberData(nameof(AuthTestDataForValidLogin))]
        public void Login_ActionExecutes_ReturnSuccessDataResultWithUser(UserForLoginDto userForLogin)
        {
            byte[] passwordHash, passwordSalt;

            HashingHelper.CreatePasswordHash(userForLogin.Password, out passwordHash, out passwordSalt);

            User userForLoginTest = new User()
            {
                Email = "userlogintest@test.com",
                FirstName = "User Login Test",
                LastName = "User Login Test",
                Status = true,
                UserId = 1,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            _moqUserService.Setup(x => x.GetByEmail(userForLogin.Email)).Returns(new SuccessDataResult<User>(userForLoginTest, "Kullanıcı listelendi"));

            var result = _authManager.Login(userForLogin);

            var returnDataResult = Assert.IsType<SuccessDataResult<User>>(result);

            _moqUserService.Verify(x => x.GetByEmail(userForLogin.Email), Times.Once);
            Assert.NotNull(returnDataResult.Data);
            Assert.Equal(userForLoginTest.PasswordSalt, returnDataResult.Data.PasswordSalt);
            Assert.True(returnDataResult.Success);
            Assert.Equal("Giriş başarılı", returnDataResult.Message);
        }

        [Theory]
        [MemberData(nameof(AuthTestDataForUserEmailInValidLogin))]
        public void Login_UserEmailIsInvalid_ReturnErrorDataResultWith(UserForLoginDto userForLogin)
        {
            _moqUserService.Setup(x => x.GetByEmail(userForLogin.Email)).Returns(new SuccessDataResult<User>(null, "Kullanıcı listelendi"));

            var result = _authManager.Login(userForLogin);

            var returnDataResult = Assert.IsType<ErrorDataResult<User>>(result);

            _moqUserService.Verify(x => x.GetByEmail(userForLogin.Email), Times.Once);
            Assert.Null(returnDataResult.Data);
            Assert.False(returnDataResult.Success);
            Assert.Equal("Parola veya kullanıcı adı hatalı", returnDataResult.Message);
        }

        [Theory]
        [MemberData(nameof(AuthTestDataForUserPasswordInValidLogin))]
        public void Login_UserPasswordIsInvalid_ReturnErrorDataResult(UserForLoginDto userForLogin)
        {
            byte[] passwordHash, passwordSalt;

            HashingHelper.CreatePasswordHash("mysupersecretkey06", out passwordHash, out passwordSalt);

            User userForLoginTest = new User()
            {
                Email = "userlogintest@test.com",
                FirstName = "User Login Test",
                LastName = "User Login Test",
                Status = true,
                UserId = 1,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            _moqUserService.Setup(x => x.GetByEmail(userForLogin.Email)).Returns(new SuccessDataResult<User>(userForLoginTest, "Kullanıcı listelendi"));

            var result = _authManager.Login(userForLogin);

            var returnDataResult = Assert.IsType<ErrorDataResult<User>>(result);

            _moqUserService.Verify(x => x.GetByEmail(userForLogin.Email), Times.Once);
            Assert.Null(returnDataResult.Data);
            Assert.False(returnDataResult.Success);
            Assert.Equal("Parola veya kullanıcı adı hatalı", returnDataResult.Message);
        }

        public static IEnumerable<object[]> AuthTestDataForValidCreateAccessToken()
        {
            yield return new object[] { new User() { UserId = 1, Email = "usertest@test.com" , FirstName = "User Test Name", LastName = "User Test LastName",
                Status = true, PasswordHash = CreateSpecialByteArray(1), PasswordSalt = CreateSpecialByteArray(1)
                }
            };
        }

        public static IEnumerable<object[]> AuthTestDataForValidRegister()
        {
            yield return new object[] {
                new UserForRegisterDto { Email = "usertest1@test1.com", Password = "mysupersecretpassword01", FirstName = "User Test 1 Name", LastName = "User Test 1 LastName" }, "mysupersecretpassword01"
            };
        }

        public static IEnumerable<object[]> AuthTestDataForValidLogin()
        {
            yield return new object[] {
                new UserForLoginDto {Email = "userlogintest@test.com", Password = "mysupersecretkey33"}
            };
        }

        public static IEnumerable<object[]> AuthTestDataForUserEmailInValidLogin()
        {
            yield return new object[] {
                new UserForLoginDto {Email = "usertest@test.com", Password = "mysupersecretkey06"}
            };
        }

        public static IEnumerable<object[]> AuthTestDataForUserPasswordInValidLogin()
        {
            yield return new object[] {
                new UserForLoginDto {Email = "userlogintest@test.com", Password = "mysupersecretkey03"}
            };
        }

        public static byte[] CreateSpecialByteArray(int length)
        {
            var arr = new byte[length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = 0x20;
            }
            return arr;
        }
    }
}
