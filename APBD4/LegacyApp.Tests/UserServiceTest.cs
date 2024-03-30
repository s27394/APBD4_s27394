using System;
using JetBrains.Annotations;
using LegacyApp;
using Xunit;

namespace LegacyApp.Tests;

[TestSubject(typeof(UserService))]
public class UserServiceTest
{
    [Fact]
    public void AddUser_method_should_return_true_in_correct_use_case()
    {
        // ARRANGE
        UserService userService = new UserService();
        // ACT
        var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        // ASSERT
        Assert.True(addResult);
    }

    [Fact]
    public void AddUser_method_should_return_false_when_first_name_is_missing()
    {
        // ARRANGE
        UserService userService = new UserService();
        // ACT
        var addResult = userService.AddUser("", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        // ASSERT
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_method_should_return_false_when_last_name_is_missing()
    {
        // ARRANGE
        UserService userService = new UserService();
        // ACT
        var addResult = userService.AddUser("John", "", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        // ASSERT
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_method_should_return_false_when_email_doesnt_have_at_dot_and_at_sign()
    {
        // ARRANGE
        UserService userService = new UserService();
        // ACT
        var addResult = userService.AddUser("John", "Doe", "johndoegmailcom", DateTime.Parse("1982-03-21"), 1);
        // ASSERT
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_method_should_return_false_when_user_is_under_21_yo()
    {
        // ARRANGE
        UserService userService = new UserService();
        // ACT
        var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Parse("2004-03-21"), 1);
        // ASSERT
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_method_should_return_false_when_user_will_be_21_yo_this_year_but_isnt_now()
    {
        // ARRANGE
        UserService userService = new UserService();
        // ACT
        var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Parse("2003-12-21"), 1);
        // ASSERT
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_method_should_return_false_when_user_will_be_21_yo_this_month_but_isnt_now()
    {
        // ARRANGE
        UserService userService = new UserService();
        // ACT
        var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Parse("2003-03-31"), 1);
        // ASSERT
        Assert.False(addResult);
    }


    [Fact]
    public void AddUser_method_should_throw_exception_when_user_is_not_found_in_db()
    {
        // ARRANGE
        UserService userService = new UserService();
        // ACT
        // // ASSERT
        Assert.Throws<ArgumentException>(() =>
        {
            var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Parse("2000-12-21"), 999);
        });
    }

    [Fact]
    public void AddUser_method_should_return_true_if_client_is_very_important()
    {
        // ARRANGE
        var userService = new UserService();
        ClientRepository clientRepository = new ClientRepository();
        // ACT
        User user = new User();
        var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 2);
        // ASSERT
        Assert.True(addResult);
    }

    [Fact]
    public void AddUser_method_should_return_false_if_user_has_credit_limit_lower_than_500()
    {
        // ARRANGE
        var userService = new UserService();
        ClientRepository clientRepository = new ClientRepository();
        // ACT
        User user = new User();
        var addResult = userService.AddUser("John", "Kowalski", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        // ASSERT
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_method_should_return_true_if_user_is_important()
    {
        // ARRANGE
        var userService = new UserService();
        ClientRepository clientRepository = new ClientRepository();
        // ACT
        User user = new User();
        var addResult = userService.AddUser("John", "Kowalski", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 4);
        // ASSERT
        Assert.True(addResult);
    }

}