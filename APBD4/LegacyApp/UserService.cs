using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (
                !(ValidateFirstNameInput(firstName) &&
                  ValidateLastNameInput(lastName) &&
                  ValidateEmailInput(email) &&
                  VerifyIfUserIsAnAdult(dateOfBirth)))
            {
                return false;
            }

            var newUser = CreateUser(firstName, lastName, email, dateOfBirth, clientId);

            if (UserHasLargeEnoughCreditLimit(newUser))
            {
                UserDataAccess.AddUser(newUser);
                return true;
            }

            return false;
        }

        private bool UserHasLargeEnoughCreditLimit(User newUser){
            if (newUser.HasCreditLimit && newUser.CreditLimit < 500)
            {
                return false;
            }
            return true;
        }

        private User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            var client = RetrieveClientFromDbById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            SetCreditLimit(user, client.Type);
            return user;
        }

        private void SetCreditLimit(User user, string clientType)
        {
            if (clientType == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (clientType == "ImportantClient")
            {
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = 2 * userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }
        }

        private Client RetrieveClientFromDbById(int clientId)
        {
            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);
            return client;
        }

        private bool ValidateFirstNameInput(string firstName)
        {
            return !string.IsNullOrEmpty(firstName);
        }

        private bool ValidateLastNameInput(string lastName)
        {
            return !string.IsNullOrEmpty(lastName);
        }

        private bool ValidateEmailInput(string email)
        {
            if (email.Contains("@") && email.Contains("."))
            {
                return true;
            }

            return false;

        }

        private bool VerifyIfUserIsAnAdult(DateTime dateOfBirth)
        {
            var age = GetUsersAge(dateOfBirth);
            return age >= 21;
        }

        private int GetUsersAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (!DidUserHaveBirthdayThisYear(now, dateOfBirth))
            {
                age--;
            }
            return age;
        }

        private bool DidUserHaveBirthdayThisYear(DateTime now, DateTime dateOfBirth)
        {
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                return false;
            }

            return true;
        }
    }
}
