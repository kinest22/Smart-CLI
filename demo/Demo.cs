using System;
using System.Collections.Generic;
using System.Globalization;
using SmartCLI;
using SmartCLI.Commands;



// new command space
var testCmdSpace = CommandSpace.ConfigureNew(cmdspace =>
{
    // command space properties
    cmdspace.HasName("Test Command Space")
    .HasDescription("This is test.");

    // adding new command to command space
    cmdspace.NewCommand<TestParams>(cmd =>
    {
        // cmd properties
        cmd.HasName("main")
        .HasDescription("this is main command.")
        .HasRoutine(para => CommandRoutine(para))
        .IsHidden();

        // arg 1 (numeric)
        cmd.HasNumericArg(para => para.Id)
        .WithName("ID")
        .WithDescription("user ID.")
        .WithMinValue(1)
        .WithNumberStyle(NumberStyles.Any)
        .WithFormatProvider(CultureInfo.InvariantCulture);

        // arg 2 (date)
        cmd.HasDateTimeArg(para => para.BirthDate)
        .WithName("birthdate")
        .WithDescription("date of birth.")
        .WithStartDate(new DateTime(2000, 1, 1));

        // arg 3 (string)
        cmd.HasStringArg(para => para.Email!)
        .WithName("Email")
        .WithDescription("User email (gmail).")
        .WithRegex(@"\w*@gmail.com$");

        // arg 4 (collection)
        cmd.HasCollectionArg(para => para.Values)
        .WithName("numbers")
        .WithDescription("Lucky numbers.")
        .WithMaxCapacity(5)
        .WithValidation(d => d <= 100);
    });
});

var cmd = testCmdSpace.Commands[0];
Console.WriteLine("General cmd info:");
Console.WriteLine($"name: {cmd.Name}");
Console.WriteLine($"desc: {cmd.Description}");
Console.WriteLine("\nExecution result:");
cmd.Execute("13 05.18.2001 kinest@gmail.com 22 7 30 18 88");

// target routine to execute.
static void CommandRoutine(TestParams @params)
{
    Console.WriteLine($"ID is {@params.Id}");
    Console.WriteLine($"Birth date: {@params.BirthDate}");
    Console.WriteLine($"Email is {@params.Email}");
    foreach(var val in @params.Values)
        Console.WriteLine($"\tlucky number {val}");
}


/// <summary>
///     Represents test command parameters.
/// </summary>
public class TestParams : VoidParams
{
    public int Id { get; set; }
    public DateTime BirthDate { get; set; }
    public string? Email { get; set; }
    public ICollection<double> Values { get; set; } = Array.Empty<double>();
}