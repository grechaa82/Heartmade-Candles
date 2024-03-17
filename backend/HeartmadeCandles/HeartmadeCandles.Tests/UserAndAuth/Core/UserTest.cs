using Bogus;
using Bogus.DataSets;
using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System;

namespace HeartmadeCandles.UnitTests.UserAndAuth.Core;

public class UserTest
{
    private static readonly Faker _faker = new("ru");

    [Theory]
    [MemberData(nameof(GenerateTestDataForCreateValidParameters))]
    public void Create_ValidParameters_ReturnsSuccess(
        int id,
        string email,
        string userName,
        string passwordHash,
        DateTime registrationDate)
    {
        // Arrange

        // Act
        var result = User.Create(
            id : id,
            email : email,
            userName : userName,
            passwordHash : passwordHash,
            registrationDate: registrationDate);

        // Assert
        Assert.True(result.IsSuccess);
    }

    public static IEnumerable<object[]> GenerateTestDataForCreateValidParameters()
    {
        var faker = new Faker();
        var person = faker.Person;

        for (var i = 0; i < 100; i++)
            yield return new object[]
            {
                GenerateData.GenerateId(),
                person.Email,
                person.UserName,
                person.Avatar,
                faker.Date.Past()
            };
    }

    [Theory]
    [MemberData(nameof(GenerateTestDataForCreateValidParameters))]
    public void Create_UserNameIsCreatedFromEmail_ReturnsSuccess(
        int id,
        string email,
        string userName,
        string passwordHash,
        DateTime registrationDate)
    {
        // Arrange
        if (userName.Length > User.MaxUserNameLength)
        {
            userName.Substring(0, User.MaxUserNameLength);
        }

        var newEmail = userName + "@gmail.com";

        // Act
        var result = User.Create(
           email: newEmail,
           passwordHash: passwordHash);


        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(userName, result.Value.Email);
        Assert.True(result.Value.UserName.Length <= User.MaxUserNameLength);
    }

    private static Result<User> Make(
        int? id = null,
        string? email = null,
        string? userName = null,
        string? passwordHash = null,
        DateTime? registrationDate = null)
    {
        var faker = new Faker();
        var person = faker.Person;

        return User.Create(
            id: id ?? GenerateData.GenerateId(),
            email : email  ?? person.Email,
            userName: userName ?? person.UserName,
            passwordHash : passwordHash  ?? person.Avatar,
            registrationDate: registrationDate ?? faker.Date.Past());
    }
}
