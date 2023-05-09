using Chat.BLL.DTO;
using Chat.BLL.Models;
using Chat.BLL.Models.Requests;
using Chat.BLL.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chat.Tests.BLL.Validators.Tests
{
    [TestFixture]
    public class ValidationTests
    {

        [TestCase(null, null, false)]
        [TestCase("name", null, false)]
        [TestCase(null, "desc", false)]
        [TestCase("VeeeryLongNaaaaame", null, false)]
        [TestCase("Name", "d", false)]
        [TestCase("Name", "Normal description", true)]
        public void CreateNewGropupValidator(string? name, string? description, bool isValid)
        {
            //arrange
            var _validator = new CreateGroupValidator();
            var newGroup = new CreateGroupRequest() 
            {
                Name = name,
                Description = description
            };
            //act
            var result = _validator.Validate(newGroup);
            //assert
            Assert.That(result.IsValid, Is.EqualTo(isValid) );
        }

        [TestCase(null, null, null, null, false)]
        [TestCase("email@email", null, null, null, false)]
        [TestCase(null, "pass", null, null, false)]
        [TestCase(null, null, "newPass", null, false)]
        [TestCase(null, null, null, "passConf", false)]
        [TestCase("notEmail", "pass", "newPass", "newPass", false)]
        [TestCase("email@email", "pass", "newPass", "notEqual", false)]
        [TestCase("email@email", "pass", "pass", "pass", false)]
        [TestCase(null, "pass", "newPass", "newPass", false)]
        [TestCase("email@email", "pass", "newPass", "newPass", true)]
        [TestCase("email@email", null, "newPass", "newPass", false)]
        [TestCase("email@email", "pass", null, "newPass", false)]
        [TestCase("email@email", "pass", "newPass", null, false)]
        public void UserChangePassValidator(string? email, string? oldPass, string? newPass, string? newPassConfirm, bool isValid)
        {
            //arrange
            var _validator = new UserChangePassValidator();
            var vhangePassModel = new ChangePasswordModel()
            {
                Email = email,
                OldPassword = oldPass,
                NewPassword = newPass,
                NewPasswordConfirm = newPassConfirm 
            };
            //act
            var result = _validator.Validate(vhangePassModel);
            //assert
            Assert.That(result.IsValid, Is.EqualTo(isValid));
        }

        [TestCase(null, null, null, null, null, null, false)]
        [TestCase("Firstname", null, null, null, null, null, false)]
        [TestCase(null, "LastName", null, null, null, null, false)]
        [TestCase(null, null, "email@email", null, null, null, false)]
        [TestCase(null, null, null, null, null, "+380993799092", false)]
        [TestCase("Firstname", "LastName", "email@email", null, null, "+380993799092", true)]
        [TestCase("F", "LastName", "email@email", null, null, "+380993799092", false)]
        [TestCase("Firstname", "L", "email@email", null, null, "+380993799092", false)]
        [TestCase("Firstname", "LastName", "notEmail", null, null, "+380993799092", false)]
        [TestCase("Firstname123", "LastName", "notEmail", null, null, "+380993799092", false)]
        [TestCase("Firstname", "LastName321", "email@email", null, null, "+380993799092", false)]
        [TestCase("Firstname", "LastName", "email@email", "Nickname", null, "+380993799092", true)]
        [TestCase("Firstname", "LastName", "email@email", "__==//Nickname//__", null, "+380993799092", true)]
        [TestCase("Firstname", "LastName", "email@email", "n", null, "+380993799092", false)]
        [TestCase("Firstname", "LastName", "email@email", null, null, "+380103799092", false)]
        [TestCase("Firstname", "LastName", "email@email", "Nickname", "2010-01-01", "+380993799092", true)]
        [TestCase("Firstname", "LastName", "email@email", "Nickname", "2023-01-01", "+380993799092", false)]
        [TestCase("Firstname", "LastName", "email@email", "Nickname", "1800-01-01", "+380993799092", false)]
        public void UserDTOValidator(
            string? firstName, 
            string? lastName, 
            string? email, 
            string? nickname, 
            string? birthDate, 
            string? phonenumber, 
            bool isValid)
        {
            //arrange
            var _validator = new UserDTOValidator();
            var userModel = new UserDTO()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Nickname = nickname,
                PhoneNumber = phonenumber
            };

            if (birthDate != null)
            {
                userModel.BirthDate = DateTime.Parse(birthDate);
            }
            //act
            var result = _validator.Validate(userModel);
            //assert
            Assert.That(result.IsValid, Is.EqualTo(isValid));
        }

        [TestCase(null, null, false)]
        [TestCase("email@email", null, false)]
        [TestCase("notEmail", "pass", false)]
        [TestCase("email@email", "pass", true)]

        public void UserLoginValidator(string? email, string? password, bool isValid)
        {
            //arrange
            var _validator = new UserLoginValidator();
            var vhangePassModel = new UserLoginModel()
            {
                Email = email,
                Password = password
            };
            //act
            var result = _validator.Validate(vhangePassModel);
            //assert
            Assert.That(result.IsValid, Is.EqualTo(isValid));
        }

        [TestCase(null, null, null, null, null, null, null, null, false)]
        [TestCase("Firstname", null, null, null, null, null, null, null, false)]
        [TestCase(null, "LastName", null, null, null, null, null, null, false)]
        [TestCase(null, null, "email@email", null, null, null, null, null, false)]
        [TestCase(null, null, null, null, null, "+380993799092", null, null, false)]
        [TestCase("Firstname", "LastName", "email@email", null, null, "+380993799092", "password", "password", true)]
        [TestCase("Firstname", "LastName", "email@email", null, null, "+380993799092", "password", null, false)]
        [TestCase("Firstname", "LastName", "email@email", null, null, "+380993799092", null, "password", false)]
        [TestCase("F", "LastName", "email@email", null, null, "+380993799092", "password", "password", false)]
        [TestCase("Firstname", "L", "email@email", null, null, "+380993799092", "password", "password", false)]
        [TestCase("Firstname", "LastName", "notEmail", null, null, "+380993799092", "password", "password", false)]
        [TestCase("Firstname123", "LastName", "notEmail", null, null, "+380993799092", "password", "password", false)]
        [TestCase("Firstname", "LastName321", "email@email", null, null, "+380993799092", "password", "password", false)]
        [TestCase("Firstname", "LastName", "email@email", "Nickname", null, "+380993799092", "password", "password", true)]
        [TestCase("Firstname", "LastName", "email@email", "__==//Nickname//__", null, "+380993799092", "password", "password", true)]
        [TestCase("Firstname", "LastName", "email@email", "__==//Nickname//__", null, "+380993799092", "password", "wrongPass", false)]
        [TestCase("Firstname", "LastName", "email@email", "n", null, "+380993799092", "password", "password", false)]
        [TestCase("Firstname", "LastName", "email@email", null, null, "+380103799092", "password", "password", false)]
        [TestCase("Firstname", "LastName", "email@email", "Nickname", "2010-01-01", "+380993799092", "password", "password", true)]
        [TestCase("Firstname", "LastName", "email@email", "Nickname", "2023-01-01", "+380993799092", "password", "password", false)]
        [TestCase("Firstname", "LastName", "email@email", "Nickname", "1800-01-01", "+380993799092", "password", "password", false)]
        public void UserRegistrationValidator(
            string? firstName,
            string? lastName,
            string? email,
            string? nickname,
            string? birthDate,
            string? phonenumber,
            string? password,
            string? confirmPass,
            bool isValid)
        {
            //arrange
            var _validator = new UserRegistrationValidator();
            var userModel = new UserRegistrationModel()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Nickname = nickname,
                PhoneNumber = phonenumber,
                Password = password,
                ConfirmPassword = confirmPass
            };

            if (birthDate != null)
            {
                userModel.BirthDate = DateTime.Parse(birthDate);
            }
            //act
            var result = _validator.Validate(userModel);
            //assert
            Assert.That(result.IsValid, Is.EqualTo(isValid));
        }
        

    }
}
