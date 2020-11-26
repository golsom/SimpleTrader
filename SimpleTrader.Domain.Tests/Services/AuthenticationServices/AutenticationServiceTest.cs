using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using SimpleTrader.Domain.Exceptions;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.Domain.Services.AuthenticationServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTrader.Domain.Tests.Services.AuthenticationServices
{
    [TestFixture]
    public class AutenticationServiceTest
    {
        private Mock<IAccountService> _mockAccountService;
        private Mock<IPasswordHasher> _mockPasswordHasher;
        private AutenticationService _autenticationService;

        [SetUp]
        public void SetUp()
        {
            _mockAccountService = new Mock<IAccountService>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _autenticationService = new AutenticationService(_mockAccountService.Object, _mockPasswordHasher.Object);

        }

        [Test]
        public async Task Login_WithCorrectPasswordForExistingUsername_ReturnsAccountForCorrectUsername()
        {
            // Arrange
            string exceptedUsername = "testuser";
            string password = "testpassword";
            _mockAccountService.Setup(s => s.GetByUsername(exceptedUsername)).ReturnsAsync(new Account() { AccountHolder = new User() { Username = exceptedUsername } });
            _mockPasswordHasher.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password)).Returns(PasswordVerificationResult.Success);

            //Act
            Account account = await _autenticationService.Login(exceptedUsername, password);

            //Assert
            string actualUsername = account.AccountHolder.Username;
            Assert.AreEqual(exceptedUsername, actualUsername);
        }

        [Test]
        public void Login_WithIncorectPasswordForExistingUsername_TrowsInvalidPasswordExceptionForUsername()
        {
            string exceptedUsername = "testuser";
            string password = "testpassword";            
            _mockAccountService.Setup(s => s.GetByUsername(exceptedUsername)).ReturnsAsync(new Account() { AccountHolder = new User() { Username = exceptedUsername } });            
            _mockPasswordHasher.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password)).Returns(PasswordVerificationResult.Failed);            
            
            InvalidPasswordExeption exeption = Assert.ThrowsAsync<InvalidPasswordExeption>(() => _autenticationService.Login(exceptedUsername, password));
            
            string actualUsername = exeption.Username;
            Assert.AreEqual(exceptedUsername, actualUsername);
        }

        [Test]
        public void Login_WithNonExistingUsername_TrowsInvalidPasswordExceptionForUsername()
        {
            string exceptedUsername = "testuser";
            string password = "testpassword";
            _mockPasswordHasher.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password)).Returns(PasswordVerificationResult.Failed);
            
            UserNotFoundException exeption = Assert.ThrowsAsync<UserNotFoundException>(() => _autenticationService.Login(exceptedUsername, password));
            
            string actualUsername = exeption.Username;
            Assert.AreEqual(exceptedUsername, actualUsername);
        }

        [Test] 
        public async Task Register_WithPasswordsNotMatching_ReturnsPasswordsDoNotMatch()
        {
            string password = "testpassword";
            string confirmPassword = "confirmtestpassword";

            RegistrationResult expected = RegistrationResult.PasswordsDoNotMatch;

            RegistrationResult actual = await _autenticationService.Register(It.IsAny<string>(), It.IsAny<string>(), password, confirmPassword);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Register_WithAlreadyExsistsEmail_ReturnsEmailAlreadyExsists()
        {
            string email = "test@gmail.com";
            _mockAccountService.Setup(s => s.GetByEmail(email)).ReturnsAsync(new Account());
            RegistrationResult expected = RegistrationResult.EmailAlreadyExsists;

            RegistrationResult actual = await _autenticationService.Register(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Register_WithAlreadyExsistsUsername_ReturnsUsernameAlreadyExsists()
        {
            string username = "test";
            _mockAccountService.Setup(s => s.GetByUsername(username)).ReturnsAsync(new Account());
            RegistrationResult expected = RegistrationResult.UsernameAlreadyExsists;

            RegistrationResult actual = await _autenticationService.Register(It.IsAny<string>(), username, It.IsAny<string>(), It.IsAny<string>());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Register_WithNonExistingUserAndMatchingPassword_ReturnsSuccess()
        {           
            RegistrationResult expected = RegistrationResult.Success;

            RegistrationResult actual = await _autenticationService.Register(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            Assert.AreEqual(expected, actual);
        }
    }
}
