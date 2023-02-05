using System.Text.RegularExpressions;
using TBC.Task.API.Models;

namespace TBC.Task.API.Extensions;

public static class PersonValidationExtensions
{
	public static bool IsValid(this RequestPersonModel person) =>
		IsFirstNameValid(person) &&
		IsLastNameValid(person) &&
		IsPersonalNumberValid(person) &&
		IsBirthDateValid(person) &&
		IsMobilePhoneValid(person) &&
		IsHomePhoneValid(person) &&
		IsWorkPhoneValid(person);

	public static bool IsFirstNameValid(this RequestPersonModel person) =>
		IsNamesValid(person.FirstName);

	public static bool IsLastNameValid(this RequestPersonModel person) =>
		IsNamesValid(person.LastName);

	public static bool IsPersonalNumberValid(this RequestPersonModel person) =>
		Regex.IsMatch(person.PersonalNumber, "^[0-9]{11}$");

	public static bool IsBirthDateValid(this RequestPersonModel person) =>
		person.BirthDate.HasValue && GetAge(person.BirthDate.Value) >= 18;

	public static bool IsWorkPhoneValid(this RequestPersonModel person) =>
		person.MobilePhone == null || Regex.IsMatch(person.MobilePhone, @"^.{4,50}$");

	public static bool IsMobilePhoneValid(this RequestPersonModel person) =>
		person.WorkPhone == null || Regex.IsMatch(person.WorkPhone, @"^.{4,50}$");

	public static bool IsHomePhoneValid(this RequestPersonModel person) =>
		person.HomePhone == null || Regex.IsMatch(person.HomePhone, @"^.{4,50}$");

	private static bool IsNamesValid(string? input)
	{
		input ??= string.Empty;
		bool hasValidLength = Regex.IsMatch(input, @"^.{2,50}$");
		bool hasLatinPattern = Regex.IsMatch(input, @"^[a-zA-Z]+$");
		bool hasGeorgianPattern = Regex.IsMatch(input, @"^[ა-ჰ]+$");

		return hasValidLength &&
			   (hasLatinPattern || hasGeorgianPattern) &&
			   !(hasLatinPattern && hasGeorgianPattern);
	}

	private static int GetAge(DateTime bornDate)
	{
		DateTime today = DateTime.Today;
		int age = today.Year - bornDate.Year;
		if (bornDate > today.AddYears(-age))
		{
			age--;
		}

		return age;
	}
}
