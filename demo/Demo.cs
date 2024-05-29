#pragma warning disable CS8321 // Local function is declared but never used

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
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
        .HasRoutine(async para => await RoutineAsync(para))
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

        cmd.HasDateTimeOpt(para => para.WeddingDate)
        .WithName("wedding date");

        cmd.HasNumericOpt(para => para.Weight)
        .WithName("wedding date");
    });
});


Console.WriteLine("\nExecution result:");
testCmdSpace.Commands[0].Execute("13 05.18.2001 kinest@gmail.com 22 7 30 18 88");


// target routine to execute.
static void Routine(TestParams @params)
{
    Console.WriteLine($"ID is {@params.Id}");
    Console.WriteLine($"Birth date: {@params.BirthDate}");
    Console.WriteLine($"Email is {@params.Email}");
    foreach(var val in @params.Values)
        Console.WriteLine($"\tlucky number {val}");
}


// target asynchronous routine
static async Task RoutineAsync(TestParams @params)
{
    Console.WriteLine("Possible commands are:");
    foreach (var c in @params.PossibleCommands)
        Console.WriteLine(c.Name);

    Console.WriteLine($"Caller command is: {@params.Caller?.Name}");

    Console.WriteLine("counting to 2 sec...");
    await Task.Delay(2000);
    Console.WriteLine($"ID is {@params.Id}");
    Console.WriteLine($"Birth date: {@params.BirthDate}");
    Console.WriteLine($"Email is {@params.Email}");
    foreach (var val in @params.Values)
        Console.WriteLine($"\tlucky number {val}");
}


/// <summary>
///     Represents test command parameters.
/// </summary>
public class TestParams : VoidParams
{
    public int Id { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime? WeddingDate { get; set; }
    public string? Email { get; set; }
    public ICollection<double> Values { get; set; } = Array.Empty<double>();

    public double? Weight { get; set; }
}