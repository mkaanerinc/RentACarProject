using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Security.JWT;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace Test.ApiTests
{
    public class AuthApiControllerTest
    {
        private readonly Mock<IAuthService> _moqService;
        private readonly AuthController _authController;
        private User _user;
        private AccessToken _accessToken;

        public AuthApiControllerTest()
        {
            _moqService = new Mock<IAuthService>();
            _authController = new AuthController(_moqService.Object);
            _user = new User()
            {
                UserId = 1,
                Email = "usertest@test.com",
                FirstName = "John",
                LastName = "Doe",
                PasswordHash = CreateSpecialByteArray(1),
                PasswordSalt = CreateSpecialByteArray(1),
                Status = true
            };
            _accessToken = new AccessToken()
            {
                Expiration = new DateTime(2023, 11, 8, 10, 15, 33),
                Token = "testtoken"
            };
        }

        [Theory]
        [MemberData(nameof(AuthTestDataForValidRegister))]
        public void Register_ActionExecutes_ReturnSuccessDataResultWithAccessToken(UserForRegisterDto userForRegister)
        {
            _moqService.Setup(x => x.UserExists(userForRegister.Email)).Returns(new SuccessResult());

            _moqService.Setup(x => x.Register(userForRegister, userForRegister.Password)).Returns(new SuccessDataResult<User>(_user, "Kullanıcı kayıt edildi."));

            _moqService.Setup(x => x.CreateAccessToken(_user)).Returns(new SuccessDataResult<AccessToken>(_accessToken, "Access Token oluşturuldu."));

            var result = _authController.Register(userForRegister);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnData = Assert.IsAssignableFrom<AccessToken>(okResult.Value);

            Assert.NotNull(returnData);
        }

        [Theory]
        [MemberData(nameof(AuthTestDataForInValidRegister))]
        public void Register_EmailExists_ReturnErrorResult(UserForRegisterDto userForRegister)
        {
            _moqService.Setup(x => x.UserExists(userForRegister.Email)).Returns(new ErrorResult("Kullanıcı kayıtlıdır."));

            var result = _authController.Register(userForRegister);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            var returnResult = Assert.IsAssignableFrom<string>(badRequestResult.Value);

            Assert.Equal("Kullanıcı kayıtlıdır.", returnResult);
        }

        [Theory]
        [MemberData(nameof(AuthTestDataForValidLogin))]
        public void Login_ActionExecutes_ReturnSuccessDataResultWithToken(UserForLoginDto userForLogin)
        {
            _moqService.Setup(x => x.Login(userForLogin)).Returns(new SuccessDataResult<User>(_user, "Giriş başarılı."));

            _moqService.Setup(x => x.CreateAccessToken(_user)).Returns(new SuccessDataResult<AccessToken>(_accessToken, "Access Token oluşturuldu."));

            var result = _authController.Login(userForLogin);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnData = Assert.IsAssignableFrom<AccessToken>(okResult.Value);

            Assert.NotNull(returnData);
        }

        [Theory]
        [MemberData(nameof(AuthTestDataForInValidLogin))]
        public void Login_UserInfoIsInvalid_ReturnErrorDataResult(UserForLoginDto userForLogin)
        {
            _moqService.Setup(x => x.Login(userForLogin)).Returns(new ErrorDataResult<User>("Parola veya kullanıcı adı hatalı."));

            var result = _authController.Login(userForLogin);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            var returnMessage = Assert.IsAssignableFrom<string>(badRequestResult.Value);

            Assert.Equal("Parola veya kullanıcı adı hatalı.", returnMessage);
        }

        public static IEnumerable<object[]> AuthTestDataForValidRegister()
        {
            yield return new object[] { new UserForRegisterDto { Email = "usertest1@test1.com", Password = "mysupersecretpassword01", FirstName = "User Test 1 Name", LastName = "User Test 1 LastName" } };
        }

        public static IEnumerable<object[]> AuthTestDataForValidLogin()
        {
            yield return new object[] { new UserForLoginDto { Email = "usertest@test.com", Password = "mysupersecretpassword06" } };
        }

        public static IEnumerable<object[]> AuthTestDataForInValidRegister()
        {
            yield return new object[] { new UserForRegisterDto { Email = "usertest@test.com", Password = "mysupersecretpassword01", FirstName = "User Test 1 Name", LastName = "User Test 1 LastName" } };
        }

        public static IEnumerable<object[]> AuthTestDataForInValidLogin()
        {
            yield return new object[] { new UserForLoginDto { Email = "usertest@test.com", Password = "mysupersecretpassword01" } };
        }

        public static byte[] CreateSpecialByteArray(int length)
        {
            var arr = new byte[length];
            for(int i = 0; i < arr.Length; i++)
            {
                arr[i] = 0x20;
            }
            return arr;
        }
    }
}
